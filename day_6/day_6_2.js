const fishList = require("./day_6_1_input.json");
const { range, startTimer, endTimer } = require("../utils");

function toStates(fishList) {
  return fishList.reduce((map, fish) => {
    map[fish]++;
    return map;
  }, Array(9).fill(0));
}

function afterDay(stateMap) {
  const newState = [...stateMap];

  const nowZero = newState.shift();
  newState.push(0);
  newState[6] += nowZero;
  newState[8] += nowZero;

  return newState;
}

function stateAfter(days, fishList) {
  const states = toStates(fishList);
  return range(1, days).reduce((state) => afterDay(state), states);
}

function fishAfter(days, fishList) {
  return stateAfter(days, fishList).reduce((sum, current) => sum + current, 0);
}

const startTime = startTimer();
const population = fishAfter(256, fishList);
const endTime = endTimer(startTime);

console.log(`${population} in ${endTime}ms`);
