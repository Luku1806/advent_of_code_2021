const map = require("./input.json");

function coordinate({ x, y }) {
  return `${x},${y}`;
}

function node(x, y) {
  return { x, y };
}

function neigbours(map, { x, y }) {
  const directions = [
    [1, 0],
    [0, 1],
    [-1, 0],
    [0, -1],
  ];

  return directions.reduce((neigbours, [xOffset, yOffset]) => {
    const inBounds =
      y + yOffset >= 0 &&
      y + yOffset < map.length &&
      x + xOffset >= 0 &&
      x + yOffset < map[y].length;

    return inBounds
      ? [...neigbours, node(x + xOffset, y + yOffset)]
      : neigbours;
  }, []);
}

function shortestPath(map) {
  const start = node(0, 0);
  const end = node(map.length - 1, map[0].length - 1);

  const risk = new Map([[coordinate(start), 0]]);
  let toVisit = [start];

  while (toVisit.length) {
    const point = toVisit[0];
    const currentRisk = risk.get(coordinate(point));

    toVisit = neigbours(map, point).reduce((queue, { x, y }) => {
      const nextCoordinate = coordinate({ x, y });
      const nextRisk = risk.get(nextCoordinate);

      if (!nextRisk || currentRisk + map[y][x] < nextRisk) {
        risk.set(nextCoordinate, currentRisk + map[y][x]);
        return [...queue, node(x, y)];
      }

      return queue;
    }, toVisit.splice(1));
  }

  return risk.get(coordinate(end));
}

module.exports = {
  shortestPath,
};
