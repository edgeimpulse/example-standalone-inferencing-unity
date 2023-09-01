# Edge Impulse Example: Inferencing plugin for Unity

This builds an exported impulse locally on your machine as a shared library. See the documentation at [Deploy your model as a C++ library](https://docs.edgeimpulse.com/docs/deploy-your-model-as-a-c-library).
Library can then be imported in Unity as a new asset.

## Basic steps

 * Download and unzip your Edge Impulse C++ library into this directory
 * Enter `./build.sh` in this directory to compile the project
 * Copy the `Unity/EIInference.cs` script in your Unity project. This script provides 2 examples to run inferencing on static data
 * Import the `build/libedgeimpulse.so` as a New Asset in your Unity project (Alternatively, import the unitypackage inside the `Unity` directory)


 ## License

 [Appache License v2.0](https://www.apache.org/licenses/LICENSE-2.0)