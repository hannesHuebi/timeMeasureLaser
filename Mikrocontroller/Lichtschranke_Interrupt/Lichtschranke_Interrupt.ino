/*
  DigitalReadSerial
 Reads a digital input on pin 2, prints the result to the serial monitor

 This example code is in the public domain.
 */

// digital pin 2 has a pushbutton attached to it. Give it a name:
int lichtschranke1 = 2;
int lichtschranke2 = 3;
int LED = 13;
int LedLichtschranke1 = 7;
int LedLichtschranke2 = 8;
int debounceTime = 3500;
int refreshTimeInMS = 300;
String readString;
long timeStart, timeEnd, deltaTime,interTime;
volatile int counter = 0;
volatile bool useInterrupt1 = false;
volatile bool useInterrupt2 = false;
// the setup routine runs once when you press reset:
void setup() {
  // initialize serial communication at 9600 bits per second:
  Serial.begin(9600);
  // make the pushbutton's pin an input:
  pinMode(lichtschranke1, INPUT_PULLUP);
  pinMode(lichtschranke2, INPUT_PULLUP);
  pinMode(LED, OUTPUT);
  pinMode(LedLichtschranke1, OUTPUT);
  pinMode(LedLichtschranke2, OUTPUT);
  digitalWrite(LED, LOW);
  digitalWrite(LedLichtschranke1, LOW);
  digitalWrite(LedLichtschranke2, LOW);
}

// the loop routine runs over and over again forever:
void loop() {
  startLoop:
  detachInterrupt(0);
  detachInterrupt(1);
  useInterrupt1 = false;
  useInterrupt2 = false;
  counter = 0;
  digitalWrite(LED, LOW);
  // read the input pin:

      readString = ckeckSerialWord();
  if (readString.length() >0) {
    if (readString.indexOf("time1") > -1)
    {
      digitalWrite(LED, HIGH);
      
      attachInterrupt(0, catchInterruptTime1, RISING);
      
      useInterrupt1 = true;
      
      while (counter < 2)
      {
        if (ckeckSerialWord().length() >0) {
          goto startLoop;
        }
        interTime = timeStart;
        while (counter == 1)
        {
          if (ckeckSerialWord().length() >0) {
            goto startLoop;
          }
          deltaTime = millis()-interTime;
          if (deltaTime > refreshTimeInMS)
          {
            interTime = millis();
            deltaTime = interTime - timeStart;
            String sendLine = "time1:";
            sendLine+=deltaTime;
            Serial.println(sendLine);
          }
        }
      }
      detachInterrupt(0);
      useInterrupt1 = false;
      useInterrupt2 = false;
      counter = 0;
      digitalWrite(LED, LOW);
      deltaTime = timeEnd-timeStart;
      
      String sendLine = "timeend1:";
      sendLine+=deltaTime;
      
      Serial.println(sendLine); //see what was received
      counter = 0;
    }
    else if (readString.indexOf("time2") > -1)
    {
      digitalWrite(LED, HIGH);
      
      attachInterrupt(0, catchInterruptTime21, RISING);
      attachInterrupt(1, catchInterruptTime22, RISING);
      
      useInterrupt1 = true;
      
      while (counter < 2)
      {
        if (ckeckSerialWord().length() >0) {
          goto startLoop;
        }
        interTime = timeStart;
        while (counter == 1)
        {
          if (ckeckSerialWord().length() >0) {
            goto startLoop;
          }
          deltaTime = millis()-interTime;
          if (deltaTime > refreshTimeInMS)
          {
            interTime = millis();
            deltaTime = interTime - timeStart;
            String sendLine = "time2:";
            sendLine+=deltaTime;
            Serial.println(sendLine);
          }
        }
      }
      
      
      detachInterrupt(0);
      detachInterrupt(1);
      useInterrupt1 = false;
      useInterrupt2 = false;
      counter = 0;
      digitalWrite(LED, LOW);
      
      deltaTime = timeEnd-timeStart;
      
      String sendLine = "timeend2:";
      sendLine+=deltaTime;
      
      Serial.println(sendLine); //see what was received

    }
    else
    {
      Serial.println(readString);
    }
    readString="";
  }
  if (digitalRead(lichtschranke1) ==0)
  {
    digitalWrite(LedLichtschranke1, HIGH);
  }
  else
  {
    digitalWrite(LedLichtschranke1, LOW);
  }
  
  if (digitalRead(lichtschranke2) == 0)
  {
    digitalWrite(LedLichtschranke2, HIGH);
  }
  else
  {
    digitalWrite(LedLichtschranke2, LOW);
  }

  delay(1);        // delay in between reads for stability
}

String ckeckSerialWord()
{
  String returnString = "";
  while (Serial.available()) {
  delay(10);  //delay to allow buffer to fill 
  if (Serial.available() >0) {
      char c = Serial.read();  //gets one byte from serial buffer
      returnString += c; //makes the string readString
    } 
  }
  return returnString;
}

void catchInterruptTime1()
{
  if (useInterrupt1)
  {
    if (counter == 0)
    {
      timeStart = millis();
      counter++;
    }
    else if (counter == 1 && (millis() - timeStart) > debounceTime)
    {
      timeEnd = millis();
      counter++;
    }
    else
    {
      
    }
  }
}
void catchInterruptTime21()
{
  if (useInterrupt1)
  {
    if (counter == 0)
    {
      timeStart = millis();
      useInterrupt2 = true;
      useInterrupt1 = false;
      counter++;
    }
  }
}
void catchInterruptTime22()
{
  if (useInterrupt2)
  {
    if (counter == 1 && (millis() - timeStart) > debounceTime)
    {
      timeEnd = millis();
      counter++;
    }
    else
    {
      
    }
  }
}



