using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;


public class EIInference : MonoBehaviour
{

    // Structure to store Edge Impulse classification result
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EiClassification
    {
        public string label;
        public float value;
    }

    // Import Edge Impulse lib as plugin
    [DllImport("Assets/Plugins/libedgeimpulse.so")]
    private static extern EiClassification ei_inference(double[] rawData, int bufSize);

    // Start is called before the first frame update
    void Start()
    {

        EiClassification results;
        print("Edge Impulse Standalone Inferencing: ");

        // Copy raw features here from your sensor (e.g. from the 'Model testing' page)
        // Here we have an up-down sample (source: https://studio.edgeimpulse.com/public/14299/latest/classification#load-sample-123974785)
        double[] rawDataUpDown = {2.2600, -1.2700, -1.5300, 1.9500, -1.7500, -1.1900, 1.7900, -2.8500, 0.6500, 1.9100, -2.9100, 2.3500, 1.9100, -2.9100, 2.3500, 1.9900, -2.4100, 3.5900, 1.2700, -0.3800, 2.5200, 1.5300, 0.9900, 2.7300, 2.0200, 0.3000, 4.1400, 1.2400, -0.9500, 5.8300, 0.7400, -1.2500, 6.8400, 0.7400, -1.2500, 6.8400, 0.3100, -0.4200, 6.1200, 1.1300, 0.2500, 8.0100, 1.9600, 0.7400, 10.2900, 1.4600, 0.8700, 9.3800, 0.1200, -0.3400, 9.1400, 0.5500, -1.2900, 12.6500, 0.5500, -1.2900, 12.6500, 1.1000, -1.0500, 14.3300, 0.8300, -0.4800, 12.2600, 0.1900, -0.7900, 11.1900, -0.0500, -0.8100, 12.9700, -0.4500, -0.3500, 17.0500, -0.1000, 0.8300, 17.1800, -0.1000, 0.8300, 17.1800, -0.6000, 0.4200, 14.1100, -0.9000, 0.9200, 12.1500, -0.6000, 1.4200, 14.1400, -0.6200, 1.3100, 15.6600, -0.4800, 1.8700, 14.5600, -0.4300, 1.5300, 13.2100, -0.4300, 1.5300, 13.2100, -0.7800, 1.1400, 12.7500, -0.9100, 1.2100, 13.0900, -0.1600, 1.2200, 14.3900, -0.2900, 1.4000, 13.6000, -1.0200, 1.3800, 13.1400, -1.3400, 0.3400, 14.9300, -1.3400, 0.3400, 14.9300, -1.0000, -1.2300, 19.1200, -1.1200, -3.4000, 19.9800, -1.2800, -2.9000, 19.9800, -1.2400, -1.8300, 19.9800, -1.6200, -2.3900, 19.9800, -1.3900, -1.9000, 19.9800, -1.3900, -1.9000, 19.9800, -1.5300, -1.6200, 19.9800, -1.4700, -0.6200, 19.3400, -1.0400, 1.1900, 17.5700, -1.0400, 1.6700, 15.1700, -1.0600, 1.9200, 12.6000, -0.5100, 3.3900, 12.1800, -0.5100, 3.3900, 12.1800, 0.1600, 3.3500, 14.2200, -0.4200, 1.8600, 13.8700, -0.7000, 1.4700, 10.9600, 0.0000, 2.0200, 8.0200, 0.8800, 2.7000, 6.6000, 1.1700, 2.2400, 7.5300, 1.1700, 2.2400, 7.5300, 0.8600, 0.7700, 9.3000, 1.1300, 0.6500, 9.8600, 0.7000, 0.7600, 6.6400, 0.0000, 0.6500, 2.3700, 0.3300, 0.5600, 0.9900, 0.3300, 0.5600, 0.9900, 1.0200, 0.3000, 1.5100, 0.6200, -0.5900, 1.2600, 0.6800, -1.4500, 1.1200, 0.6900, -2.6900, -0.0900, 0.9100, -2.7700, -1.8600, 1.5000, -2.4300, -2.2100, 1.5000, -2.4300, -2.2100, 2.0400, -3.0300, 0.7100, 2.3600, -2.7900, 3.4400, 2.6200, -2.0500, 3.0800, 2.6000, -1.9000, 0.8300, 2.2700, -2.4700, -0.8600, 2.1600, -3.1600, -1.0300, 2.1600, -3.1600, -1.0300, 2.6200, -2.4300, 0.8500, 3.0000, -1.5500, 2.2800, 2.5900, -0.5700, 2.5000, 1.4900, -0.4300, 1.3700, 0.8600, -0.3800, 2.5600, 1.4900, 0.7300, 4.6100, 1.4900, 0.7300, 4.6100, 1.5300, 1.4200, 4.7200, 1.0800, 1.6100, 4.6600, -0.0700, 1.2600, 5.4800, 0.6200, 1.2700, 8.5500, 1.3700, 0.7600, 11.1200, 1.1400, 0.4600, 10.8000, 1.1400, 0.4600, 10.8000, 0.6400, 1.5400, 10.0600, 0.3400, 0.5300, 10.9900, 0.7600, 1.0000, 14.3200, 0.6400, 2.5200, 15.9600, 0.4300, 2.6400, 17.3400, -0.5500, 1.2500, 13.2400, -0.5500, 1.2500, 13.2400, 0.2300, 2.8200, 14.0200, 0.1300, 3.6500, 15.3500, 0.2300, 3.7600, 16.5700, -0.2900, 2.9500, 15.0900, -0.6000, 3.7800, 15.0900, -0.1100, 4.4300, 15.5600, -0.1100, 4.4300, 15.5600, -0.6600, 3.3100, 16.1400, -0.8000, 2.6700, 16.0700, 0.0900, 3.9500, 16.8200, -0.5600, 4.1900, 16.9800, -0.3700, 3.6100, 19.5200, -0.0300, 1.5200, 19.9800, -0.0300, 1.5200, 19.9800, -0.4200, -0.2900, 19.9800, -0.5100, -0.1200, 19.3700, -0.2500, 0.4800, 19.3000, -0.2500, 0.4300, 19.5900, -0.2700, 0.5000, 19.3400, -0.2700, 0.5000, 17.6800, -0.3400, 1.2800, 17.6800, -0.3200, 2.7600, 15.9600, -0.2200, 3.3800, 14.8100};
        
        // Run inferencing
        results = ei_inference(rawDataUpDown, rawDataUpDown.Length);
        Debug.Log(results.label);
        Debug.Log(results.value);

        // snake movement sample (source: https://studio.edgeimpulse.com/public/14299/latest/classification#load-sample-123974783)
        double [] rawDataSnake = {1.5100, 3.7600, 10.2400, 1.8200, 5.4800, 9.9500, 2.0400, 6.9800, 10.2900, 1.5100, 6.5000, 9.9300, 0.2500, 4.6300, 10.0000, -1.2100, 2.4600, 10.0500, -1.2100, 2.4600, 10.0500, -2.1000, 1.0100, 9.7800, -2.2600, -0.8700, 10.1400, -1.6200, -0.8400, 10.1600, -0.9100, -0.2000, 10.2300, -0.3500, 0.3600, 10.0600, 1.8100, 0.8200, 9.4900, 1.8100, 0.8200, 9.4900, 2.5600, -1.9800, 9.1300, 2.0700, -4.9000, 10.1000, 0.3800, -7.3400, 10.5400, 0.6000, -5.7300, 9.9900, 0.8400, -3.4300, 10.1800, 1.0900, -3.7900, 9.7000, 1.0900, -3.7900, 9.7000, 1.3500, -4.2600, 10.0400, 1.6000, -3.5800, 9.9400, 1.2400, -1.3200, 9.6000, 1.3500, -1.2600, 9.7600, 1.1300, -1.9200, 10.1400, 0.3200, -0.7300, 10.2100, 0.3200, -0.7300, 10.2100, 2.9500, -0.8200, 9.9000, 3.4400, 2.1700, 10.0200, 2.5300, 3.6700, 10.1800, 1.6400, 3.9100, 9.7000, 0.5100, 4.5900, 9.9800, 0.1600, 5.0400, 10.0900, 0.1600, 5.0400, 10.0900, 1.0600, 4.7000, 10.3300, 1.5900, 4.4000, 9.9400, 0.3900, 3.0200, 10.2600, 0.1000, 1.7800, 9.9900, -0.3700, 1.2800, 10.1500, -0.3700, 1.2800, 9.9100, -0.2000, 1.5200, 9.9100, -0.3300, 2.5200, 9.9600, 0.3600, 3.2800, 10.0300, 0.2300, 3.3400, 10.3300, -1.1100, 1.7400, 9.9100, -2.6200, -1.1100, 10.2900, -2.6200, -1.1100, 10.2900, -1.8700, -3.1100, 10.0000, -2.0900, -4.0700, 9.9400, -2.1200, -4.0100, 10.2100, -1.4100, -4.2000, 10.0900, -1.6900, -3.5100, 10.1500, 0.3500, -3.0000, 10.1300, 0.3500, -3.0000, 10.1300, -0.0500, -2.7300, 9.9400, 0.0200, -3.1800, 10.0700, 0.9200, -1.9000, 10.1700, 2.2100, -1.8700, 9.9700, 2.9500, -2.5600, 9.9800, 1.9400, -3.2000, 10.2200, 1.9400, -3.2000, 10.2200, 1.3400, -2.5700, 9.6700, 1.4300, -0.1200, 10.1100, 1.4300, 1.7100, 9.8200, 1.0400, 3.2000, 9.7500, 1.4900, 4.2400, 10.0100, 0.4400, 4.5200, 9.9900, 0.4400, 4.5200, 9.9900, 0.2000, 4.6100, 10.0700, -1.7500, 4.5500, 10.1900, -2.7800, 3.8400, 9.9700, -2.4100, 6.8100, 10.2100, -2.6200, 6.6100, 10.0100, -2.5500, 3.6900, 9.9800, -2.5500, 3.6900, 9.9800, -1.5300, -0.1100, 10.0300, -0.7700, -1.3900, 10.1900, -0.7500, -0.2500, 9.9300, -0.5000, 1.5100, 10.2800, -0.3600, 1.0900, 9.8600, 0.0000, -0.0900, 9.8100, 0.0000, -0.0900, 9.8100, 0.4700, -1.2800, 9.8700, -0.3000, -1.4700, 9.8900, 0.0100, -1.0100, 10.0300, 0.4000, -0.3100, 9.3500, 1.4400, -1.5900, 10.1700, 3.4200, -3.8200, 9.3300, 3.4200, -3.8200, 9.3300, 3.1300, -6.0200, 9.7800, 2.7100, -5.0600, 9.9900, 2.6100, -4.3400, 9.5700, 2.8000, -2.4700, 10.1500, 2.1400, -1.5300, 10.0500, 2.1000, -0.7400, 10.1800, 2.1000, -0.7400, 10.1800, 2.2400, -0.9800, 10.0100, 1.4600, -0.8800, 9.5800, 1.1500, 0.6100, 9.9500, 0.2300, 2.2000, 9.9000, -1.6100, 3.3300, 10.1100, -1.6100, 3.3300, 10.1100, -2.0800, 4.9400, 9.4500, -2.5800, 6.0800, 9.6200, -3.5000, 4.8000, 9.5500, -3.4500, 1.6200, 9.9600, -1.4000, 1.1200, 9.3100, 0.0100, 0.4700, 10.1500, 0.0100, 0.4700, 10.1500, 0.8300, 0.7500, 9.8500, 0.4000, 2.8100, 10.0700, 1.2100, 1.7800, 9.6500, 1.5000, 0.7100, 10.0000, 0.8300, 1.3800, 9.9000, 0.4700, 1.3400, 9.7300, 0.4700, 1.3400, 9.7300, 1.0200, 1.4500, 9.9100, 0.4200, 0.5000, 10.0600, -0.0500, -0.7300, 9.6900, 0.1500, -1.0500, 9.6400, 1.0700, -1.8400, 10.1000, 1.1500, -2.8100, 9.7100, 1.1500, -2.8100, 9.7100, 0.3500, -3.4200, 9.9100};
        results = ei_inference(rawDataSnake, rawDataSnake.Length);
        Debug.Log(results.label);
        Debug.Log(results.value);

    }

    // Update is called once per frame
    void Update()
    {       
    }

}
