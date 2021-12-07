const positions = require("./positions.json");

function range(start, end) {
  return Array(Math.max(start, end) - Math.min(start, end) + 1)
    .fill(0)
    .map((_, index) => Math.min(start, end) + index);
}

function calculateFuelConsumption(positions, position) {
  return positions.reduce(
    (sum, current) => sum + Math.abs(current - position),
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

console.log(calculateLeastConsumption(positions));
