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
#define PRINT_PERIOD_LIMIT_MS 20  // Just in case, limit USB-serial printing to no more than once every this number of milliseconds.
#define N_PACKETS 3  // Number of packets to capture before analyzing
#define PRINT_CHANNELS 16  // Number of channels to print to USB serial
#define FIRST_CHANNEL 2  // Packet index of first channel
#define MIN_BREAK_MICROS 200  // A break between packets will be at least this many microseconds

uint8_t packets[PACKET_SIZE * N_PACKETS];  // Buffer to store the received packets
uint8_t values[N_PACKETS];  // Buffer to count values for a channel across packets
uint8_t counts[N_PACKETS];  // Buffer to count values for a channel across packets
uint8_t printValues[PRINT_CHANNELS];  // Currently-accepted "printable" values
size_t packetOffset = 0;
size_t byteIndex = 0;
bool capturingPacket = false;
unsigned long lastPrint;
unsigned long lastByteTime;

void setup() {
  Serial.begin(115200);  // For interfacing with PC
  Serial.println("@DMXReaderSketch 0.2.0");
  Serial.flush();
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
      packets[packetOffset + byteIndex++] = byte;
    } else {
      Serial.println("!Packet overflow");
      capturingPacket = false;
    }
  }

  // Check for break condition (idle line for more than MIN_BREAK_MICROS)
  if (capturingPacket && (micros() - lastByteTime > MIN_BREAK_MICROS)) {
    // End of packet detected
    if (byteIndex >= FIRST_CHANNEL + PRINT_CHANNELS) {
      // Minimum number of bytes for a valid packet obtained
      packetOffset += PACKET_SIZE;
    } else {
      Serial.print("!Packet too small: ");
      Serial.println(byteIndex);
    }
    if (packetOffset >= PACKET_SIZE * N_PACKETS) {
      // All N_PACKETS have been collected
      packetOffset = 0;
      printChannels();
    }
    capturingPacket = false;
  }
}

void printChannels() {
  size_t n = PRINT_CHANNELS < byteIndex ? PRINT_CHANNELS : byteIndex;
  for (size_t c = 0; c < n; c++) {
    for (size_t i = 0; i < N_PACKETS; i++) {
      counts[i] = 0;
    }
    for (size_t p = 0; p < N_PACKETS; p++) {
      uint8_t v = packets[p * PACKET_SIZE + FIRST_CHANNEL + c];
      for (uint8_t i = 0; i < N_PACKETS; i++) {
        if (counts[i] == 0 || values[i] == v) {
          values[i] = v;
          counts[i]++;
          break;
        }
      }
    }
    uint8_t bestCount = 0;
    for (uint8_t i = 0; i < N_PACKETS; i++) {
      if (counts[i] > bestCount) {
        printValues[c] = values[i];
        bestCount = counts[i];
      }
      if (counts[i] >= N_PACKETS / 2 + 1) {
        break;
      }
    }
    printValues[c] = packets[(N_PACKETS - 1) * PACKET_SIZE + FIRST_CHANNEL + c];
  }
  if (millis() >= lastPrint + PRINT_PERIOD_LIMIT_MS) {
    for (size_t c = 0; c < n; c++) {
      if (printValues[c] < 0x10) {
        Serial.print('0');
      }
      Serial.print(printValues[c], HEX);
      Serial.print(' ');
    }
    Serial.println();
    lastPrint += PRINT_PERIOD_LIMIT_MS;
  }
}
