const { toCoordinatesOnLine } = require("./day_5");

function coordinate(x, y) {
  return { x, y };
}

describe("Day 5 Part 1", () => {
  describe("coordinatesBetween should", () => {
    it("return right coordinates for simple input with straight line", () => {
      const line = { xStart: 1, yStart: 1, xEnd: 1, yEnd: 2 };

      console.log(toCoordinatesOnLine(line));

      expect(toCoordinatesOnLine(line)).toEqual([
        coordinate(1, 1),
        coordinate(1, 2),
      ]);
    });

    it("return right coordinates for longer input with straight line", () => {
      const line = { xStart: 1, yStart: 1, xEnd: 5, yEnd: 1 };

      console.log(toCoordinatesOnLine(line));

      expect(toCoordinatesOnLine(line)).toEqual([
        coordinate(1, 1),
        coordinate(2, 1),
        coordinate(3, 1),
        coordinate(4, 1),
        coordinate(5, 1),
      ]);
    });

    it("return right coordinates for longer input with straight line", () => {
      const line = { xStart: 1, yStart: 1, xEnd: 3, yEnd: 3 };

      expect(toCoordinatesOnLine(line)).toEqual([
        coordinate(1, 1),
        coordinate(2, 2),
        coordinate(3, 3),
      ]);
    });
  });
});
