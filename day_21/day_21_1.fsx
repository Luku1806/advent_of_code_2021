#load "../utils.fsx"
open utils

type Player =
    {
        position: int
        score: int
    }

    static member init position = { position = position - 1; score = 0 }

    static member move ({ position = position; score = score }: Player) (diceRoll: int) : Player =
        let newPosition = (position + diceRoll) % 10

        {
            position = newPosition
            score = score + newPosition + 1
        }

let diceRolls =
    Seq.initInfinite (fun throw -> (throw * 3 + 2) * 3)

let play (player1: Player) (player2: Player) =
    let rec innerPlay (player1: Player) (player2: Player) (moveNumber: int) =
        match moveNumber % 2, player1, player2 with
        | _, { score = s1 }, { score = s2 } when s1 >= 1000 || s2 >= 1000 -> (player1, player2, moveNumber)
        | 0, _, _ -> innerPlay (Player.move player1 (diceRolls |> Seq.item moveNumber)) player2 (moveNumber + 1)
        | 1, _, _ -> innerPlay player1 (Player.move player2 (diceRolls |> Seq.item moveNumber)) (moveNumber + 1)
        | _ -> failwith "Invalid move"

    innerPlay player1 player2 0

let solve () =
    play (Player.init 1) (Player.init 2)
    |> tap (fun (player1, player2, moves) -> printfn "Part 1: %d" ((min player1.score player2.score) * moves * 3))

solve ()
