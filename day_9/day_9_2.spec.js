const { calculateBasin, basinsOf } = require("./day_9_2");

const EXAMPLE_MAP = [
  [2, 1, 9, 9, 9, 4, 3, 2, 1, 0],
  [3, 9, 8, 7, 8, 9, 4, 9, 2, 1],
  [9, 8, 5, 6, 7, 8, 9, 8, 9, 2],
  [8, 7, 6, 7, 8, 9, 6, 7, 8, 9],
  [9, 8, 9, 9, 9, 6, 5, 6, 7, 8],
];

describe("Day 9 Part 2", () => {
  describe("calulateBasin", () => {
    it("should calculate basin for the top left", () => {
      expect(calculateBasin(EXAMPLE_MAP, [{ x: 1, y: 0 }]).length).toEqual(3);
    });

    it("should calculate basin for the top right", () => {
      expect(calculateBasin(EXAMPLE_MAP, [{ x: 9, y: 0 }]).length).toEqual(9);
    });

    it("should calculate basin for the middle", () => {
      expect(calculateBasin(EXAMPLE_MAP, [{ x: 2, y: 2 }]).length).toEqual(14);
    });
  });

  describe("basinsOf", () => {
    it("should return as many basins as low points exist", () => {
      expect(basinsOf(EXAMPLE_MAP).length).toEqual(4);
    });
  });
});
