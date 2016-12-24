In order to create the mbedeth lib, these files need to be compiled. They were modified
due to an issue with using static constructors in C++ that call into the Llilum runtime. 

The only change made was adding an init method to Semaphore, and calling that method from
the EthernetInterface init methods.

The following command creates the lib: <ARM_GCC_TOOLCHAIN_PATH>\bin\arm-none-eabi-ar.exe --target=elf32-little  -v -q libmbedeth.a Semaphore.o EthernetInterface.o

This lib needs to be placed into the ARM_GCC_TOOLCHAIN folder for every board

The files in this folder are found in <mbed_dir>\mbed\libraries\net\eth\EthernetInterface AND <mbed_dir>\mbed\libraries\rtos\rtos (for Semaphore)

Steps:
1. Export a sample application (for GCC) from mBed that uses EthernetInterface
2. Find Samephore.h and add the method header "void init(void);"
3. Open Semaphore.cpp and comment out this line in the constructor: "_osSemaphoreId = osSemaphoreCreate(&_osSemaphoreDef, count);"
4. Add a method in Semaphore.cpp that looks like the following:
void Semaphore::init(void)
{

	_osSemaphoreId = osSemaphoreCreate(&_osSemaphoreDef, _count);

}
5. Replace EthernetInterface.cpp and EthernetInterface.h with the ones from <llilum_root>\llilum\Zelig\mbed\EthernetInterface
6. Run make in the sample directory
7. Copy Semaphore.o and EthernetInterface.o into one folder
8. Run <ARM_GCC_TOOLCHAIN_PATH>\bin\arm-none-eabi-ar.exe --target=elf32-little  -v -q libmbedeth.a Semaphore.o EthernetInterface.o