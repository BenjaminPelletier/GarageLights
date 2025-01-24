#include <SoftwareSerial.h>

#define RX_PIN 3  // Pin where the serial data is received
#define PACKET_SIZE 200  // Maximum packet size (bytes / DMX channels + overhead)
#define BAUD_RATE 250000
#define PRINT_PERIOD_LIMIT_MS 20
#define N_PACKETS 5  // Number of packets to capture before analyzing
#define PRINT_CHANNELS 16  // Number of channels to print to USB serial
#define FIRST_CHANNEL 2  // Packet index of first channel

SoftwareSerial softSerial(RX_PIN, -1);  // RX only

uint8_t packets[PACKET_SIZE * N_PACKETS];  // Buffer to store the received packets
uint8_t values[N_PACKETS];  // Buffer to count values for a channel across packets
uint8_t counts[N_PACKETS];  // Buffer to count values for a channel across packets
uint8_t printValues[PRINT_CHANNELS];  // Currently-accepted "printable" values
size_t packetOffset = 0;
size_t byteIndex = 0;
bool capturingPacket = false;
unsigned long lastPrint;

void setup() {
  Serial.begin(115200);  // For interfacing with PC
  Serial.println("@DMXReaderSketch 0.1.0");
  Serial.flush();
  pinMode(RX_PIN, INPUT);
  softSerial.begin(BAUD_RATE);
  lastPrint = millis();
  Serial.println("@Starting");
}

void loop() {
  static unsigned long lastByteTime = 0;

  // Read bytes if available
  while (softSerial.available()) {
    if (!capturingPacket) {
      // Start a new packet
      capturingPacket = true;
      byteIndex = 0;
    }

    // Read the next byte
    uint8_t byte = softSerial.read();
    lastByteTime = millis();

    // Store the byte in the buffer
    if (byteIndex < PACKET_SIZE) {
      packets[packetOffset + byteIndex++] = byte;
    } else {
      Serial.println("!Packet overflow");
      capturingPacket = false;
    }
  }

  // Check for break condition (idle line for more than 10ms)
  if (capturingPacket && (millis() - lastByteTime > 10)) {
    // End of packet detected
    packetOffset += PACKET_SIZE;
    if (packetOffset >= PACKET_SIZE * N_PACKETS) {
      packetOffset = 0;
      if (byteIndex >= FIRST_CHANNEL + PRINT_CHANNELS) {
        printChannels();
      } else {
        Serial.print("!Packet too small: ");
        Serial.println(byteIndex);
      }
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
    // Serial.print("C"); Serial.print(c); Serial.print(": ");
    uint8_t bestCount = 0;
    for (uint8_t i = 0; i < N_PACKETS; i++) {
      // Serial.print(values[i], HEX); Serial.print("@"); Serial.print(counts[i]); Serial.print(' ');
      if (counts[i] > bestCount) {
        printValues[c] = values[i];
        bestCount = counts[i];
      }
      if (counts[i] >= N_PACKETS / 2 + 1) {
        // Serial.print("-> "); Serial.print(printValues[c], HEX);
        break;
      }
    }
    // Serial.println();
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
