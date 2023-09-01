#include <stdio.h>
#include "edge-impulse-sdk/classifier/ei_run_classifier.h"

#if _MSC_VER // this is defined when compiling with Visual Studio
#define EXPORT_API __declspec(dllexport) // Visual Studio needs annotating exported functions with this
#else
#define EXPORT_API // XCode does not need annotating exported functions, so define is empty
#endif

// Link following functions C-style (required for plugins)
extern "C"
{

// Callback function declaration
static int get_signal_data(size_t offset, size_t length, float *out_ptr);

// Declare features array
static float features[EI_CLASSIFIER_DSP_INPUT_FRAME_SIZE];

// Function returns class with best prediction
// Object Detection or anomaly not supported
EXPORT_API ei_impulse_result_classification_t ei_inference(double* raw_data, int buf_size) {

    // redirect stdout to debug file
    // doesn't seem to output at first run, probably Unity issue
    freopen("debug.txt", "a", stdout);
    printf("Debug Log\r\n");
    
    signal_t signal;            // Wrapper for raw input buffer
    ei_impulse_result_t result; // Used to store inference output
    EI_IMPULSE_ERROR res;       // Return code from inference

    // Make sure that the length of the buffer matches expected input length
    if (buf_size != EI_CLASSIFIER_DSP_INPUT_FRAME_SIZE) {
        printf("ERROR: The size of the input buffer is not correct.\r\n");
        printf("Expected %d items, but got %d\r\n", 
                EI_CLASSIFIER_DSP_INPUT_FRAME_SIZE, 
                (int)buf_size);
        ei_impulse_result_classification_t res_cl;
        res_cl.label = "ERROR";
        res_cl.value = -1;
        return res_cl;
    }

    // Copy raw data array passed by Unity to our static features array
    for (uint32_t i = 0; i < buf_size; i++) {
        features[i] = (float)raw_data[i];
    }

    // Assign callback function to fill buffer used for preprocessing/inference
    signal.total_length = EI_CLASSIFIER_DSP_INPUT_FRAME_SIZE;
    signal.get_data = &get_signal_data;

    // Perform DSP pre-processing and inference
    res = run_classifier(&signal, &result, false);
    printf("res: %d\r\n", res);

    // Print return code and how long it took to perform inference
    printf("run_classifier returned: %d\r\n", res);
    printf("Timing: DSP %d ms, inference %d ms, anomaly %d ms\r\n", 
            result.timing.dsp, 
            result.timing.classification, 
            result.timing.anomaly);

    // To store best classification results
    uint8_t best_class_index = 0;
    float best_class_value = 0.0;

    // Print the prediction results (object detection)
#if EI_CLASSIFIER_OBJECT_DETECTION == 1
    printf("Object detection bounding boxes:\r\n");
    for (uint32_t i = 0; i < EI_CLASSIFIER_OBJECT_DETECTION_COUNT; i++) {
        ei_impulse_result_bounding_box_t bb = result.bounding_boxes[i];
        if (bb.value == 0) {
            continue;
        }
        printf("  %s (%f) [ x: %u, y: %u, width: %u, height: %u ]\r\n", 
                bb.label, 
                bb.value, 
                bb.x, 
                bb.y, 
                bb.width, 
                bb.height);
    }

    // Print the prediction results (classification)
#else
    printf("Predictions:\r\n");
    for (uint16_t i = 0; i < EI_CLASSIFIER_LABEL_COUNT; i++) {
        printf("  %s: ", ei_classifier_inferencing_categories[i]);
        printf("%.5f\r\n", result.classification[i].value);
        // save best classification result
        if (result.classification[i].value > best_class_value) {
            best_class_index = i;
            best_class_value = result.classification[i].value;
        }
    }
#endif

    // Print anomaly result (if it exists)
#if EI_CLASSIFIER_HAS_ANOMALY == 1
    printf("Anomaly prediction: %.3f\r\n", result.anomaly);
#endif

    return result.classification[best_class_index];

}

// Callback: fill a section of the out_ptr buffer when requested
static int get_signal_data(size_t offset, size_t length, float *out_ptr) {
    for (size_t i = 0; i < length; i++) {
        out_ptr[i] = (features + offset)[i];
    }
    return EIDSP_OK;
}

}