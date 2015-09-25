/******************************************************************************
*       Description:
*
*       Author: DW
*         Date: 22 April 2015
*
*******************************************************************************/
#pragma mark Compiler Pragmas


#pragma mark Includes
#include "CardLaminator.h"
#include "States/LaminatorState.h"
#include "Thread.h"
#include "Timer.h"

using namespace VitalElement;
using namespace VitalElement::Threading;


#pragma mark Definitions and Constants
static const float CtpStepsPerRevolution = 12800.0f;
static const float RollerCircumference = 33.77212102f;
const float CardLaminator::CardTransportSpeedHigh = 55.0f * StepsPerMM;
const float CardLaminator::HeaterPositionSpeed = 8000;
const float CardLaminator::StepsPerMM =
CtpStepsPerRevolution / RollerCircumference;

#pragma mark Static Data


#pragma mark Static Functions


#pragma mark Member Implementations
CardLaminator::CardLaminator (IBoard& board)
    : UsbInterface (
      UltraLaminatorHidDevice (*board.hidDevice, mainDispatcher, settings)),
      stateEngine (StateEngine (mainDispatcher)),
      mainDispatcher (Dispatcher (*board.dispatcherActions)), board (board),
      CassetteUpper (Cassette (*board.TakeUpMotorUpper, *board.OutputMotorUpper,
                               *board.OutputMotorEncoderUpper)),
      CassetteLower (JvcCassette (*board.TakeUpMotorLower,
                                  *board.OutputMotorLower,
                                  *board.OutputMotorEncoderUpper))
{
}


CardLaminator::~CardLaminator ()
{
}


void CardLaminator::Run ()
{
    stateContext = new LaminatorStateContext (stateEngine, *this);

    Thread::StartNew (
    [&]
    {
        Initialise ();
    },
    200);

    Thread::StartNew (
    [&]
    {
        TemperatureContollerWorker ();
    },
    100);

    Thread::StartNew (
    [&]
    {
        ADCWorker ();
    },
    100);

    Thread::StartNew (
    [&]
    {
        CommsWorker ();
    },
    100);

    auto ledTimer1 = Timer::Create (50, true);
    static bool toggled1 = false;

    ledTimer1->Elapsed += [&](void*, EventArgs&)
    {
        if (toggled1)
        {
            board.LED2On ();
            toggled1 = false;
        }
        else
        {
            board.LED2Off ();
            toggled1 = true;
        }

    };

    ledTimer1->Start ();

    auto ledTimer2 = Timer::Create (1000, true);
    static bool toggled2 = false;

    ledTimer2->Elapsed += [&](void*, EventArgs&)
    {
        if (toggled2)
        {
            board.LED1On ();
            toggled2 = false;
        }
        else
        {
            board.LED1Off ();
            toggled2 = true;
        }

    };

    ledTimer2->Start ();

    while (true)
    {
        mainDispatcher.Run ();
    }
}

void CardLaminator::Initialise ()
{
    board.hidDevice->InitialiseStack ();

    board.CardTransportMotor->SetSpeed (CardTransportSpeedHigh, 0.25f, 0.25f);
    board.HopperFeedMotor->SetSpeed (3500.0f, 5.0f, 5.0f);

    board.HeaterPositionUpperMotor->SetSpeed (HeaterPositionSpeed, 0.25f,
                                              0.25f);

    board.HeaterPositionLowerMotor->SetSpeed (HeaterPositionSpeed, 0.25f,
                                              0.25f);

    UsbInterface.SettingsChanged += [&](void*, EventArgs&)
    {
        board.HeaterUpper->SetTemperature (settings.TemperatureTop.Get ());
        board.HeaterLower->SetTemperature (settings.TemperatureBottom.Get ());
    };

    int32_t heaterUpperResetPosition = -1;
    int32_t heaterLowerResetPosition = -1;

    board.HeaterPositionUpperMotor->Goto (5200);

    board.HeaterPositionLowerMotor->Goto (5200);

    while (
    board.HeaterPositionLowerMotor->state != StepperMotor::State::Stopped &&
    board.HeaterPositionUpperMotor->state != StepperMotor::State::Stopped)
    {
    }

    EventHandler* upperHandler = board.HeaterUpperResetPosition +=
    [&](void*, EventArgs&)
    {
        board.HeaterUpperResetPosition -= upperHandler;
        if (heaterUpperResetPosition == -1)
        {
            heaterUpperResetPosition = board.HeaterPositionUpperMotor->position;
            board.HeaterPositionUpperMotor->Stop ();
        }
    };

    EventHandler* lowerHandler = board.HeaterLowerResetPosition +=
    [&](void*, EventArgs&)
    {
        board.HeaterLowerResetPosition -= lowerHandler;
        if (heaterLowerResetPosition == -1)
        {
            heaterLowerResetPosition = board.HeaterPositionLowerMotor->position;
            board.HeaterPositionLowerMotor->Stop ();
        }
    };

    board.HeaterPositionUpperMotor->Start (StepperMotor::Direction::Forward);

    while (board.HeaterPositionUpperMotor->state !=
           StepperMotor::State::Stopped)
    {
    }

    board.HeaterPositionUpperMotor->Goto (heaterUpperResetPosition);

    while (board.HeaterPositionUpperMotor->state !=
           StepperMotor::State::Stopped)
    {
    }

    board.HeaterPositionLowerMotor->Start (StepperMotor::Direction::Forward);

    while (board.HeaterPositionLowerMotor->state !=
           StepperMotor::State::Stopped)
    {
    }

    board.HeaterPositionLowerMotor->Goto (heaterLowerResetPosition);

    while (board.HeaterPositionLowerMotor->state !=
           StepperMotor::State::Stopped)
    {
    }

    board.HeaterPositionUpperMotor->ResetPosition ();
    board.HeaterPositionLowerMotor->ResetPosition ();

    board.CardTransportMotor->Stop ();

    stateEngine.SetState (stateContext->Idle);
}


void CardLaminator::CommsWorker ()
{
    auto commsThread = Thread::GetCurrentThread ();

    while (true)
    {
        Thread::Sleep (100);

        mainDispatcher.BeginInvoke (
        [&]
        {
            if (UsbInterface.IsConnected ())
            {
                uint8_t stateId = 255;

                if (stateEngine.GetCurrentState () != nullptr)
                {
                    stateId =
                    (uint8_t)((LaminatorState*)stateEngine.GetCurrentState ())
                    ->id;
                }

                UsbInterface.ReportStatus (
                stateId, board.OutputMotorEncoderUpper->GetCounter (),
                board.TakeupMotorEncoderUpper->GetCounter());
            }

            commsThread->Resume ();
        });

        Thread::Suspend ();
    }
}

void CardLaminator::ADCWorker ()
{
    while (true)
    {
        board.HeaterUpper->InvalidateAdc ();
        board.HeaterLower->InvalidateAdc ();
        Thread::Yield ();
    }
}

void CardLaminator::TemperatureContollerWorker ()
{
    auto temperatureThread = Thread::GetCurrentThread ();

    while (true)
    {
        Thread::Sleep (20);

        mainDispatcher.BeginInvoke ([&]
                                    {
                                        board.HeaterUpper->Invalidate ();
                                        board.HeaterLower->Invalidate ();

                                        temperatureThread->Resume ();
                                    });

        Thread::Suspend ();
    }
}
