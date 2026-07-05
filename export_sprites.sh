#!/bin/bash

# Export card sprites from base.aseprite
# Creates one PNG for each suit+rank combination

set -e

SOURCE="./Sprites/Cards/base.aseprite"
EXPORT_DIR="./export"
RANKS=(
    "2"
    "3"
    "4"
    "5"
    "6"
    "7"
    "8"
    "9"
    "10"
    "j"
    "q"
    "k"
    "a"
    "l"
    "r"
    )

# Declare associative arrays for suits and their rank groups
declare -A SUIT_RANK_GROUP=(
    [Hearts]="RedRank"
    [Diamonds]="RedRank"
    [Spades]="BlackRank"
    [Clubs]="BlackRank"
    [Trump]="BlackRank"
)

SUITS=(
    "Hearts"
    "Diamonds"
    "Spades"
    "Clubs"
    "Trump"
    )

mkdir -p "$EXPORT_DIR"

echo "Exporting cards from $SOURCE"
echo "Output directory: $EXPORT_DIR"
echo

successful=0
total=0

# Export standard suits
for suit in "${SUITS[@]}"; do
    rank_group="${SUIT_RANK_GROUP[$suit]}"
    suit_lower=$(echo "$suit" | tr '[:upper:]' '[:lower:]')

    for rank in "${RANKS[@]}"; do
        total=$((total + 1))
        output_file="$EXPORT_DIR/${rank}_${suit_lower}.png"

        if aseprite -b "$SOURCE" \
            --layer "Base" \
            --layer "${rank_group}/${rank}" \
            --layer "Suits/${suit}" \
            --save-as "$output_file" > /dev/null 2>&1; then
            echo "✓ Exported ${rank}_${suit_lower}.png"
            successful=$((successful + 1))
        else
            echo "✗ Failed to export ${rank}_${suit_lower}.png"
        fi
    done
done

# Copy each file in ./exports to ./Sprites/Cards/
cp "$EXPORT_DIR"/* ./Sprites/Cards/

echo
echo "=================================================="
echo "Export complete: $successful/$total cards exported"
echo "Output: $(cd "$EXPORT_DIR" && pwd)/"
