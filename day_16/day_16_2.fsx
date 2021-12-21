#load "../utils.fsx"

open utils

open System

#load "day_16_1.fsx"
open part1

let rec calculate (packet: Packet) : int64 =
    let subPacketValues = packet.subPackets |> List.map calculate

    match packet.packetType with
    | 0 -> subPacketValues |> List.sum
    | 1 ->
        subPacketValues
        |> List.fold (fun product value -> product * value) 1
    | 2 -> subPacketValues |> List.min
    | 3 -> subPacketValues |> List.max
    | 4 -> defaultArg packet.literal 0
    | 5 -> if subPacketValues.[0] < subPacketValues.[1] then 1 else 0
    | 6 -> if subPacketValues.[0] > subPacketValues.[1] then 1 else 0
    | 7 -> if subPacketValues.[0] = subPacketValues.[1] then 1 else 0
    | _ -> 0

let solve =
    let packets =
        file "input.txt"
        |> toHex
        |> (fun binary -> decode Int32.MaxValue binary [])
        |> tap (fun x -> printfn "%A" x)

    calculate packets.[0]
    |> tap (fun x -> printfn "%A" x)

solve
