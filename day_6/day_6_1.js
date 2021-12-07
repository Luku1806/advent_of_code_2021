const fishList = require("./day_6_1_input.json");
const { range } = require("../utils");

function afterDay(fish) {
  return fish.reduce((allFish, fish) => {
    const newValue = fish - 1;

    if (newValue < 0) {
      allFish.push(8, 6);
    } else {
      allFish.push(newValue);
    }

    return allFish;
  }, []);
}

function fishAfter(days, fishList) {
  return range(1, days).reduce((fish, day) => {
    return afterDay(fish);
  }, fishList).length;
}

console.log(fishAfter(80, fishList));
