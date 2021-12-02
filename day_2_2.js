const { commands } = require('./day_2_1')

const { horizontal, vertical, aim } = commands.reduce(
  (result, commandWithSpeed) => {
    const { horizontal, vertical, aim } = result
    const { command, speed } = commandWithSpeed

    return {
      horizontal:
        command === 'forward'
          ? horizontal + speed
          : horizontal,
      vertical:
        command === 'forward'
          ? vertical + speed * aim
          : vertical,
      aim: command === 'down'
        ? aim + speed
        : command === 'up'
          ? aim - speed
          : aim,
    }
  },
  { horizontal: 0, vertical: 0, aim: 0 }
)

console.log(horizontal * vertical)