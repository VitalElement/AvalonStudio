/* mbed Microcontroller Library
 * Copyright (c) 2015 ARM Limited
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#ifndef MBED_LOWPOWERTIMEOUT_H
#define MBED_LOWPOWERTIMEOUT_H

#include "platform.h"

#if DEVICE_LOWPOWERTIMER

#include "lp_ticker_api.h"
#include "LowPowerTicker.h"

namespace mbed {

/** Low Power Timout
 */
class LowPowerTimeout : public LowPowerTicker {

private:
    virtual void handler(void) {
        _function.call();
    }
};

}

#endif

#endif
