const map = require("./example_input.json");
const { shortestPath } = require("./day_15_1");
const { range } = require("../utils");

function expand(map) {
  return range(0, 4).flatMap((x) =>
    map.map((row) =>
      range(0, 4).flatMap((y) =>
        row.map((value) => 1 + ((value - 1 + x + y) % 9))
      )
    )
  );
}

function expandMap(tile) {
  return range(0, 4)
    .reduce(copyDown(tile), [])
    .map((row) => range(0, 4).reduce(copyRight(row), []));
}

function copyDown(tile) {
  return (map, tileY) => [...map, ...tile.map((row) => row.map(wrap(tileY)))];
}

function copyRight(row) {
  return (map, tileX) => [...map, ...row.map(wrap(tileX))];
}

function wrap(offset) {
  return (cell) => ((cell + offset - 1) % 9) + 1;
}

console.log(shortestPath(expand(map)));
