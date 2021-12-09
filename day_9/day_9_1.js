const fs = require("fs");

function toMap(mapFile) {
  return mapFile
    .split("\n")
    .map((line) => line.trim().split(""))
    .map((row) => row.map((string) => Number.parseInt(string, 10)));
}

function depthAt(map, x, y) {
  return map[y] !== undefined && map[y][x] !== undefined ? map[y][x] : Infinity;
}

function isLowPoint(map, x, y) {
  return (
    map[y][x] < depthAt(map, x, y + 1) &&
    map[y][x] < depthAt(map, x, y - 1) &&
    map[y][x] < depthAt(map, x + 1, y) &&
    map[y][x] < depthAt(map, x - 1, y)
  );
}

function countLowPoints(map) {
  return map.reduce(
    (lowPoints, row, y) =>
      lowPoints +
      row.reduce(
        (rowLowPoints, cell, x) =>
          isLowPoint(map, x, y) ? rowLowPoints + 1 + cell : rowLowPoints,
        0
      ),
    0
  );
}

function count() {
  const mapFile = fs.readFileSync("./map.txt", "utf-8");
  const map = toMap(mapFile);
  return countLowPoints(map);
}

// console.log(count());

module.exports = {
  toMap,
  isLowPoint,
  countLowPoints,
};
