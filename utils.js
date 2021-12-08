function range(start, end) {
  return Array(Math.max(start, end) - Math.min(start, end) + 1)
    .fill(0)
    .map((_, index) => Math.min(start, end) + index);
}

function sum(array) {
  return array.reduce((sum, current) => current + sum, 0);
}

// Stolen from Alexander Rose
function startTimer() {
  return process.hrtime();
}

function endTimer(time) {
  function roundTo(decimalPlaces, numberToRound) {
    return +(
      Math.round(numberToRound + `e+${decimalPlaces}`) + `e-${decimalPlaces}`
    );
  }

  const diff = process.hrtime(time);
  const NS_PER_SEC = 1e9;
  const result = diff[0] * NS_PER_SEC + diff[1];
  const elapsed = result * 0.000001;
  return roundTo(6, elapsed);
}

module.exports = {
  range,
  startTimer,
  endTimer,
  sum,
};
