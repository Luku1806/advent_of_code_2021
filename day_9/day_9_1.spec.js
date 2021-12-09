const fs = require("fs");
const path = require("path");
const { toMap, countLowPoints } = require("./day_9_1");

const EXAMPLE_MAP = [
  [2, 1, 9, 9, 9, 4, 3, 2, 1, 0],
  [3, 9, 8, 7, 8, 9, 4, 9, 2, 1],
  [9, 8, 5, 6, 7, 8, 9, 8, 9, 2],
  [8, 7, 6, 7, 8, 9, 6, 7, 8, 9],
  [9, 8, 9, 9, 9, 6, 5, 6, 7, 8],
];

describe("Day 9 Part 1", () => {
  describe("toMap", () => {
    it("should parse example correct", () => {
      const mapFile = fs.readFileSync(
        path.resolve(__dirname, "example_map.txt"),
        "utf-8"
      );

      expect(toMap(mapFile)).toEqual(EXAMPLE_MAP);
    });
  });

  describe("countLowPoints", () => {
    it("countLowPoints should calculate the value from the example", () => {
      expect(countLowPoints(EXAMPLE_MAP)).toEqual(15);
    });
  });
});
