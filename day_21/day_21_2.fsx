#load "../utils.fsx"

open Microsoft.FSharp.Core
open utils

let diceRolls =
    seq {
        for x in 1 .. 3 do
            for y in 1 .. 3 do
                for z in 1 .. 3 do
                    x + y + z
    }

let memoize f =
    let cache = System.Collections.Generic.Dictionary<_, _>()

    (fun (player1: int) (player2: int) (score1: int) (score2: int) (p1Turn: bool) ->
        let key = (player1, player2, score1, score2, p1Turn)

        match cache.TryGetValue key with
        | true, value -> value
        | _ ->
            let value = f player1 player2 score1 score2 p1Turn
            cache.Add(key, value)
            value)


let rec play =
    memoize
        (fun (player1: int) (player2: int) (score1: int) (score2: int) (p1Turn: bool) ->
            if score1 >= 21 then
                (1L, 0L)
            else if score2 >= 21 then
                (0L, 1L)
            else
                diceRolls
                |> Seq.fold
                    (fun (totalWins1, totalWins2) dice ->
                        let w1, w2 =
                            if p1Turn then
                                let newPosition =
                                    if (player1 + dice) % 10 = 0 then
                                        10
                                    else
                                        (player1 + dice) % 10

                                let newScore = score1 + newPosition

                                play newPosition player2 newScore score2 (not p1Turn)
                            else
                                let newPosition =
                                    if (player2 + dice) % 10 = 0 then
                                        10
                                    else
                                        (player2 + dice) % 10

                                let newScore = score2 + newPosition

                                play player1 newPosition score1 newScore (not p1Turn)

                        (totalWins1 + w1, totalWins2 + w2))
                    (0L, 0L))

let solve () =
    // play 4 8 0 0 true (Example)
    play 1 2 0 0 true
    |> tap (fun (player1, player2) -> printfn "Part 2: %d" (max player1 player2))

solve ()
