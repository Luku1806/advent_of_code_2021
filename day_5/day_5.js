const fs = require("fs");

function toLines(input) {
  return input
    .split("\n")
    .map((lineString) => lineString.match(/(\d+),(\d+) -> (\d+),(\d+)/))
    .map((result) => ({
      xStart: result[1],
      yStart: result[2],
      xEnd: result[3],
      yEnd: result[4],
    }));
}

function isStraight(coordinate) {
  return (
    coordinate.xStart === coordinate.xEnd ||
    coordinate.yStart === coordinate.yEnd
  );
}

function matrix(width, heigth, defaultValue = 0) {
  return Array(width)
    .fill(defaultValue)
    .map(() => Array(heigth).fill(defaultValue));
}

function range(start, end) {
  return Array(Math.max(start, end) - Math.min(start, end) + 1)
    .fill(0)
    .map((_, index) => Math.min(start, end) + index);
}

function toCoordinatesOnLine(line) {
  if (line.xStart === line.xEnd) {
    return range(line.yStart, line.yEnd).map((y) => ({ y, x: line.xStart }));
  }

  const m = (line.yEnd - line.yStart) / (line.xEnd - line.xStart);
  const b = line.yEnd - m * line.xEnd;

  return range(line.xStart, line.xEnd).map((x) => ({
    x,
    y: m * x + b,
  }));
}

function toMap(coordinates) {
  return coordinates.reduce((map, { x, y }) => {
    map[x][y]++;
    return map;
  }, matrix(1000, 1000));
}

function intersectionsOnMap(map) {
  return map.flat().filter((value) => value > 1).length;
}

function calculateIntersectionsPart1() {
  const coordinateFile = fs.readFileSync("./coordinates.txt", "utf-8");

  const lines = toLines(coordinateFile);
  const straightLines = lines.filter(isStraight);
  const coordinates = straightLines.flatMap(toCoordinatesOnLine);
  const map = toMap(coordinates);

  return intersectionsOnMap(map);
}

function calculateIntersectionsPart2() {
  const coordinateFile = fs.readFileSync("./coordinates.txt", "utf-8");

  const lines = toLines(coordinateFile);
  const coordinates = lines.flatMap(toCoordinatesOnLine);
  const map = toMap(coordinates);

  return intersectionsOnMap(map);
}

console.log({
  part1: calculateIntersectionsPart1(),
  part2: calculateIntersectionsPart2(),
});

module.exports = {
  toCoordinatesOnLine,
};
