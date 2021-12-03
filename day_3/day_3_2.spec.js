const { lifeSupportRating, toOxygen, toCo2 } = require("./day_3_2");

const input = [
  "00100",
  "11110",
  "10110",
  "10111",
  "10101",
  "01111",
  "00111",
  "11100",
  "10000",
  "11001",
  "00010",
  "01010",
];

describe("Day 3 Part 2", () => {
  describe("toOxygen should", () => {
    it("return 23", () => {
      expect(toOxygen(input)).toEqual(23);
    });
  });

  describe("toOxygen should", () => {
    it("return 10", () => {
      expect(toCo2(input)).toEqual(10);
    });
  });

  describe("lifeSupportRating should", () => {
    it("return 230 for the example", () => {
      expect(lifeSupportRating(input)).toEqual(230);
    });
  });
});
