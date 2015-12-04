/******************************************************************************
*       Description:
*
*       Author:
*         Date: 17 July 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "Catch.h"
#include "Dispatcher.h"

using namespace VitalElement::Threading;

// TODO Add your own test cases.
TEST_CASE ("DispatcherOperation")
{
    bool criticalSectionEntered = false;
    bool criticalSectionExited = false;
    int enteredCount = 0;
    int exitedCount = 0;
    bool codeExecuted = false;

    DispatcherActions actions;

    actions.EnterCriticalSection = [&]
    {
        enteredCount++;
        criticalSectionEntered = true;
    };

    actions.ExitCriticalSection = [&]
    {
        exitedCount++;
        REQUIRE (criticalSectionEntered == true);
        criticalSectionExited = true;
    };


    Dispatcher dispatcher = Dispatcher (actions);

    // Ensure an empty dispatcher can be run....
    dispatcher.Run ();

    REQUIRE (enteredCount == 1);
    REQUIRE (exitedCount == 1);

    criticalSectionEntered = false;
    criticalSectionExited = false;

    dispatcher.BeginInvoke ([&]
                            {
                                codeExecuted = true;
                            });

    REQUIRE (criticalSectionEntered == true);
    REQUIRE (criticalSectionExited == true);
    REQUIRE (codeExecuted == false);

    criticalSectionEntered = false;
    criticalSectionExited = false;

    dispatcher.Run ();

    REQUIRE (criticalSectionEntered == true);
    REQUIRE (criticalSectionExited == true);
    REQUIRE (codeExecuted == true);
}
