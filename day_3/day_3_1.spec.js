const { toMostCommonToken } = require("./day_3_1");

describe("toMostCommonToken should", () => {
  it("should work without preference for ties", () => {
    const input = [
      ["1", "0", "1"],
      ["1", "1", "0"],
      ["0", "0", "1"],
    ];
    expect(input.map(toMostCommonToken())).toEqual(["1", "1", "0"]);
  });

  it("should work with preference 0 for ties", () => {
    const input = [
      ["1", "1", "1", "0"],
      ["1", "1", "0", "0"],
      ["0", "0", "0", "0"],
    ];

    expect(input.map(toMostCommonToken("0"))).toEqual(["1", "0", "0"]);
  });

  it("should work with preference 1 for ties", () => {
    const input = [
      ["1", "1", "1", "0"],
      ["1", "1", "0", "0"],
      ["0", "0", "0", "0"],
    ];

    expect(input.map(toMostCommonToken("1"))).toEqual(["1", "1", "0"]);
  });
});
