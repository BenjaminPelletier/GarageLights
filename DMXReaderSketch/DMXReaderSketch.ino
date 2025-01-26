// This sketch captures 250k baud 8N2 serial data from a DMX shield and reports PRINT_CHANNELS of this data to the USB serial port.
//
// Requires an Arduino Mega because Unos only have one hardware serial port.  We need one for DMX input and one for USB-serial.
// Requires a DMX shield.
// Must connect pin 3 (DMX signal from shield) to pin 17 (RX2 on AVR).
// DMX shield jumpers should be set:
//    - EN (not ~EN)
//    - Slave (not DE)
//    - TX-io (not TX-uart)
//    - RX-io (not RX-uart)

#define PACKET_SIZE 200  // Maximum packet size (bytes / DMX channels + overhead)
#define BAUD_RATE 250000  // Baud rate of DMX protocol
#define PRINT_PERIOD_LIMIT_MS 20  // Limit USB-serial printing to no more than once every this number of milliseconds.
#define PRINT_CHANNELS 16  // Number of channels to print to USB serial
#define FIRST_CHANNEL 1  // Packet index of first channel
#define MIN_BREAK_MICROS 200  // A break between packets will be at least this many microseconds

uint8_t packet[PACKET_SIZE];  // Buffer to store the received packets
size_t byteIndex = 0;
bool capturingPacket = false;
unsigned long lastPrint;
unsigned long lastByteTime;

void setup() {
  // Set up communications with PC
  Serial.begin(115200);
  Serial.println("@DMXReaderSketch 0.4.0");
  Serial.flush();

  // Set up DMX serial parsing
  Serial2.begin(BAUD_RATE, SERIAL_8N2);

  lastPrint = millis();
  Serial.println("@Starting");
}

void loop() {
  // Read bytes if available
  while (Serial2.available()) {
    // Read the next byte
    uint8_t byte = Serial2.read();
    lastByteTime = micros();

    if (!capturingPacket) {
      // Start a new packet
      capturingPacket = true;
      byteIndex = 0;
    }

    // Store the byte in the buffer
    if (byteIndex < PACKET_SIZE) {
      packet[byteIndex++] = byte;
      if (byteIndex >= FIRST_CHANNEL + PRINT_CHANNELS) {
        // Minimum number of bytes for a valid packet obtained
        printChannels();
        lastByteTime = micros();  // Make sure not to falsely detect break because we spent too much time printing channels
      }
    } else {
      Serial.println("!Packet overflow");
      capturingPacket = false;
    }
  }

  // Check for break condition (idle line for more than MIN_BREAK_MICROS)
  if (capturingPacket && (micros() - lastByteTime > MIN_BREAK_MICROS)) {
    // End of packet detected
    if (byteIndex < FIRST_CHANNEL + PRINT_CHANNELS) {
      Serial.print("!Packet too small: ");
      Serial.println(byteIndex);
    }
    capturingPacket = false;
  }
}

void printChannels() {
  if (millis() >= lastPrint + PRINT_PERIOD_LIMIT_MS) {
    size_t c1 = FIRST_CHANNEL + (PRINT_CHANNELS < byteIndex ? PRINT_CHANNELS : byteIndex);
    for (size_t c = FIRST_CHANNEL; c < c1; c++) {
      if (packet[c] < 0x10) {
        Serial.print('0');
      }
      Serial.print(packet[c], HEX);
      Serial.print(' ');
    }
    Serial.println();
    lastPrint += PRINT_PERIOD_LIMIT_MS;
  }
}
