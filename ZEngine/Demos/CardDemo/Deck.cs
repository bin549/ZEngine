using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZEngine.CardDemo;

public class Deck {
    private readonly List<string> cardNames = new List<string>();
    private readonly Random random = new Random();
    private readonly Stack<CardSprite> cards = new Stack<CardSprite>();

    public void Initialize(ContentManager content) {
        cardNames.Clear();
        string[] suits = { "club", "diamond", "heart", "spade" };
        string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        foreach (var s in suits) {
            foreach (var r in ranks) {
                cardNames.Add($"{s}_{r}");
            }
        }
        cardNames.Add("black_joker");
        cardNames.Add("red_joker");
        Build(content);
        Shuffle();
    }

    public void Shuffle() {
        var list = cards.ToList();
        cards.Clear();
        for (int i = 0; i < list.Count; i++) {
            int j = random.Next(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
        foreach (var c in list) cards.Push(c);
    }

    public CardSprite? Draw() {
        if (cards.Count == 0) return null;
        return cards.Pop();
    }

    private void Build(ContentManager content) {
        cards.Clear();
        foreach (var name in cardNames) {
            var sprite = new CardSprite();
            sprite.Load(content, name);
            cards.Push(sprite);
        }
    }
}


