# Metering #

## Overview ##

Metering designed to apply analog signal(s) to and read Modbus register(s) from the unit under test.

### Communication Page ###
---

> **Ip Address**  
    Provide "Ip Address" of the unit under test. Verify the ip address is ping-able.  

> **Port**  
    Provide Modbus protocol "Port" of the unit under test.  

> **Log**  
    Provides various information about the state of the application and ongoing tests.

### Nominal Values Page ###
---

> #### Nominal Analog Values ####
> * ##### Nominal Voltage #####
>     This entry become initial value of "Magnitude(V), From(V)" and "To(V)" in TestDetails page for every supported voltage signal.  

> * ##### Nominal Current #####
>     This entry become initial value of "Magnitude(A), From(A)" and "To(A)" in TestDetails page for every supported current signal.  

> * ##### Nominal Frequency #####
>     This entry become initial value of "Frequency(Hz), From(Hz)" and "To(Hz)" in TestDetails page for every supported signal.  

> #### Nominal Voltage Phase ####
> * ##### 0° #####
>     The initial value of "Phase(°), From(°)" and "To(°)" in TestDetails page for every supported voltage signal set to "0".  
> * ##### Balance #####
>     The initial value of "Phase(°), From(°)" and "To(°)" in TestDetails page for every supported voltage signal set to "0", "-120" and "120" per their respective phase id.  

> #### Nominal Current Phase ####
> * ##### 0° #####
>     The initial value of "Phase(°), From(°)" and "To(°)" in TestDetails page for every supported current signal set to "0".  
> * ##### Balance #####
>     The initial value of "Phase(°), From(°)" and "To(°)" in TestDetails page for every supported current signal set to "0", "-120" and "120" per their respective phase id.  

> #### Nominal Test Values ####
> * ##### Nominal Delta #####
>     The initial value of every "Delta" for each supported signal which incremented by this value per test step. 

### Commands Page ###
---
> #### Add new test steps command ####
> Populates initial values and navigates forward to "Test Details Page".  

> #### Cancel all test steps command ####
> Navigates back to "Nominal Test Page" and reset default values.  

> #### Save this test step command ####
> Saves the current "Test Details Page" values to the specified location accessible to the computer.  

> #### Load test step(s) command ####
> Loads the previously saved file(s) from the computer.  

> #### Delete selected test step from the current test strip command ####
> Removes selected test step(s) from the current test list. However this command will not delete corresponding file(s) from the computer, remove the file(s) manually deletion required.  

### Test Details Page ###
---
> #### Ramping ####
> These options would change ramping of analog signal attribute.  

> * Magnitude  
>   When this option selected, analog signal magnitude ramps from "From" to "To" value increment by "Delta" value.  

> * Phase  
>   When this option selected, analog signal phase ramps from "From" to "To" value increment by "Delta" value.  

> * Frequency  
>   When this option selected, analog signal frequency ramps from "From" to "To" value increment by "Delta" value.  

> #### Register ####
>   These comma separated value(s) would use to poll during the test steps.  

> #### Dwell Time (sec) ####
>   The duration of test steps.  

> #### Start Delay Time (min)
>   The delay time before the test step starts.

> #### Measurement Interval (msec)
>   The communication point polling interval.

> #### Start Measurement Delay (sec)
>   The delay time before the communication point polling starts.

> #### Signal ####
>   The omicron analog signal name.

> #### Magnitude (V|A) ####
>   The omicron analog signal magnitude value.

> #### Phase (°) ####
>   The omicron analog signal phase value.

> #### Frequency (Hz) ####
>   The omicron analog signal frequency value.

> #### From (V|A|°|Hz) ####
>   The test step start value.

> #### To (V|A|°|Hz) ####
>   The test step end value.

> #### Delta (V|A|°|Hz) ####
>   The test step increment value.

