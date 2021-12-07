const positions = require("./positions.json");
const { startTimer, endTimer } = require("../utils");

function range(start, end) {
  return Array(Math.max(start, end) - Math.min(start, end) + 1)
    .fill(0)
    .map((_, index) => Math.min(start, end) + index);
}

function consumptionForSteps(n) {
  return (n * (n + 1)) / 2;
}

function calculateFuelConsumption(positions, position) {
  return positions.reduce(
    (sum, current) => sum + consumptionForSteps(Math.abs(current - position)),
    0
  );
}

function calculateLeastConsumption(crabPositions) {
  return range(Math.min(...crabPositions), Math.max(...crabPositions)).reduce(
    (minimum, position) =>
      Math.min(calculateFuelConsumption(crabPositions, position), minimum),
    Number.MAX_VALUE
  );
}

const startTime = startTimer();
const population = calculateLeastConsumption(positions);
const endTime = endTimer(startTime);

console.log(`${population} in ${endTime}ms`);
