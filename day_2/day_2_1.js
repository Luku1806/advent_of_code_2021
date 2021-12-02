const fs = require('fs')
const input = fs.readFileSync('./day_2_1_input.txt', 'utf-8')

function toCommand (input) {
  const pair = input.split(' ')
  return { command: pair[0], speed: Number.parseInt(pair[1], 10) }
}

const commands = input.split('\n').map(toCommand)

const { horizontal, vertical } = commands.reduce(
  (result, commandWithSpeed) => {
    const { horizontal, vertical } = result
    const { command, speed } = commandWithSpeed

    return {
      horizontal: command === 'forward' ? horizontal + speed : horizontal,
      vertical:
        command === 'down'
          ? vertical + speed
          : command === 'up'
            ? vertical - speed
            : vertical,
    }
  },
  { horizontal: 0, vertical: 0 }
)

console.log(horizontal * vertical)

module.exports = { commands }