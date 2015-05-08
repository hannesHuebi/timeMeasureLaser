# timeMeasureLaser
This is a repository for a time measuring device based on a laser and fotodiodes. This device was meant to measure the time small model boats needs to go 25m in a towing tank.

In this repository the Schematics and PCB design for the Arduinoshield is given as necessary Hardware for the time measuring equipment.

Additionaly the Arduino code is given which measures the time and provides the commands for the serial interface. 

Based on serial communication an excel-addin is is made which reads out the time and posts it in the excel-sheet. This ribbon/addin is availeble over COM-Interop to use it somerwhere else. This is mainly done to have an interface to VBA to generate buttons to start the time measuring.
