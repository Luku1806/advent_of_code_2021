const fs = require("fs");
const { toMap, isLowPoint } = require("./day_9_1");

function byCoordinate(x, y) {
  return ({ x: currentX, y: currentY }) => currentX === x && currentY === y;
}

function isEdge(map, x, y) {
  return map[y][x] === 9;
}

function canInsert(set, x, y) {
  return !set.some(byCoordinate(x, y));
}

function calculateBasin(map, points) {
  const newPoints = points.reduce(
    (all, { x, y }) => {
      if (x > 0 && !isEdge(map, x - 1, y) && canInsert(all, x - 1, y)) {
        all.push({ x: x - 1, y });
      }

      if (
        x < map[0].length - 1 &&
        !isEdge(map, x + 1, y) &&
        canInsert(all, x + 1, y)
      ) {
        all.push({ x: x + 1, y });
      }

      if (y > 0 && !isEdge(map, x, y - 1) && canInsert(all, x, y - 1)) {
        all.push({ x, y: y - 1 });
      }

      if (
        y < map.length - 1 &&
        !isEdge(map, x, y + 1) &&
        canInsert(all, x, y + 1)
      ) {
        all.push({ x, y: y + 1 });
      }

      return all;
    },
    [...points]
  );

  return newPoints.length !== points.length
    ? calculateBasin(map, newPoints)
    : newPoints;
}

function basinsOf(map) {
  return map.reduce(
    (basins, row, y) => [
      ...basins,
      ...row.reduce(
        (rowBasins, cell, x) =>
          isLowPoint(map, x, y)
            ? [...rowBasins, calculateBasin(map, [{ x, y }])]
            : rowBasins,
        []
      ),
    ],
    []
  );
}

function count() {
  const mapFile = fs.readFileSync("./map.txt", "utf-8");
  const map = toMap(mapFile);
  const basinLengths = basinsOf(map)
    .map((basin) => basin.length)
    .sort((basin1, basin2) => basin2 - basin1);

  return basinLengths[0] * basinLengths[1] * basinLengths[2];
}

//console.log(count());

module.exports = {
  calculateBasin,
  basinsOf,
};
