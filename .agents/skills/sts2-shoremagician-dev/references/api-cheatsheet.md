# ShoreMagician API 速查与标准模板（STS2 v0.113.1 + BaseLib 3.4.8）

> 本文档中所有类型、方法与属性均已核实自 `sts2.dll` (`v0.113.1`)、`SlayTheSpire2.pck`、`WatcherMod_onlyforreference` 与当前 `ShoreMagicianCode/`。

## 1. 游戏原版核心类型映射

### 1.1 原版附魔（`MegaCrit.Sts2.Core.Models.Enchantments`）
- `Sharp`（锋利）：攻击伤害 `+{Amount}`（**受法强与翻倍增幅**）。
- `Nimble`（灵巧）：卡牌格挡 `+{Amount}`，限带格挡牌，不能附魔攻击牌（**受法强与翻倍增幅**）。
- `Adroit`（伶俐）：打出时额外获得 `{Block:diff()}` 点格挡（变量名为 `Block` 非 `Amount`），**可附魔到攻击牌上**（**受法强与翻倍增幅**）。
- `Swift`（迅速/迅捷）：第一次打出时抽 `{Amount}` 张牌（**受法强与翻倍增幅**）。
- `Sown`（播种）：每场战斗首次打出获得 `{Amount:energyIcons()}`（经 `SownOnPlayPrefix` **受法强与翻倍增幅**）。
- `Glam`（华彩）：每场战斗可重放一次（无数值，供 `淘洗`）。
- `Steady`（稳定）：卡牌获得保留（无数值，供 `海枯石烂` / `变幻打击`）。
- `PerfectFit`（完美契合）：洗入抽牌堆时置顶（供 `涌泉相报` 战后奖励）。
- `Momentum`（动量）：每次打出增加本场战斗伤害 `{Amount}`（供 `飘转` `8→10`）。
- `TezcatarasEmber`（特兹卡塔拉的余烬）：遗物 `NutritiousSoup`（营养汤）赋予的附魔；经 `NutritiousSoupAfterObtainedPostfix` 让牌组 `ManaStrike` 亦可获得。

### 1.2 原版能力 / 遗物 / 卡牌参考（`MegaCrit.Sts2.Core.Models.*`）
- **敌人状态（严禁魔改，直接沿用原版）**：`Powers.WeakPower`（虚弱）、`Powers.VulnerablePower`（易伤）、`Powers.SlowPower`（缓慢 — 全局持续，本回合每出牌令敌受攻伤 `+10%`，敌回合初自动重置回合计数）。
- **玩家能力与辅助**：`Powers.NoDrawPower`、`Powers.DrawCardsNextTurnPower`、`Powers.ArsenalPower`（储君武器库）、`Powers.IntangiblePower`（无实体）、`Powers.RetainHandPower`（保留手牌）、`Powers.DoubleDamagePower`（本回合伤害翻倍）、`StrengthPower`/`DexterityPower`（均支持负层数 `AllowNegative => true`）。
- **先古遗物与特殊牌池**：
  - **`BaseLib.Abstracts.ITranscendenceCard`**：基础卡实现该接口即自动融入原版先古遗物 `Relics.ArchaicTooth`（古老牙齿 -> `堤防`）及被 `Relics.DustyTome`（尘封魔典 -> `无定河+`）自动排除
  - **`Entities.Cards.TarotCards.All`**：原版 22 张塔罗牌列表（供 `百川归海` 使用）
  - `Cards.Enthralled`（「执迷」，供 `潇湘` 放入抽牌堆底）
  - `Cards.BorrowedTime` / `Relics.TungstenRod` / `Cards.FlakCannon`（散射炮）
  - `Cards.Fuel` / `Cards.MinionSacrifice` / `Cards.Joy` / `Cards.Soul` / `Cards.Dazed`
  - `Cards.PoorSleep`（「睡眠不佳」，用于 `入梦`）
  - `Cards.Void`（「虚空」，用于 `魔力过载`）

### 1.3 已核实命令与钩子（`MegaCrit.Sts2.Core.Commands` 与 `ShoreMagicianCmd`）
- **Mod 统一指令 (`ShoreMagicianCmd`)**：
  - `CanEnchantWith<T>(card)`（排斥已附魔 + 原版 `CanEnchant`）
  - `EnchantCard<T>(ctx, player, card, baseAmount, cardSource)`
  - **`ReplaceEnchantment<T>(ctx, player, card, baseAmount, cardSource)`**（改魔/覆盖附魔专用，只计 **1 次**变化事件，供 `聚拢冲击` / `道是无晴` / `变幻打击` 使用）
  - `ClearEnchantment(...)` / `TransformCard(...)` / `TransformToRareSameType(...)`
- **卡牌位置 / 战斗移除 / 堆顶 (`CardPileCmd`)**：
  - **`CardPileCmd.RemoveFromCombat(card, false)`**：本场战斗彻底移除（如 `回转+`）
  - **`CardPileCmd.Add(this, PileType.Draw, CardPilePosition.Top)`**：将自身放回抽牌堆顶（参考 `WatcherCode/Cards/Uncommon/Tantrum.cs`，如 `持久战`）
  - `AddGeneratedCardToCombat(card, pileType, player, pos)` + `CardCmd.PreviewCardPileAdd(...)`
  - `Draw(ctx, amount, player)`
- **卡牌进手牌监听（用于 `收束`）**：
  - `AbstractModel.AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel clonedBy)`：当被标记的 `card.Pile?.Type == PileType.Hand` 时触发一次效果并立即失效（兼容抽牌、许愿、秘密技法、清淤等一切上手方式）。
- **生物与能力 (`CreatureCmd` / `PowerCmd`)**：
  - `CreatureCmd.GainBlock(...)` / `CreatureCmd.Damage(...)` / `CreatureCmd.Stun(creature)`
  - `PowerCmd.Apply<TPower>(ctx, target, amount, applier, cardSource)` / `PowerCmd.Decrement(power)` / `PowerCmd.Remove(power)`

## 2. BaseLib `ConstructedCardModel` 常用构造器与派生技巧

```csharp
WithDamage(baseDmg, upgradeDelta);
WithBlock(baseBlk, upgradeDelta);
WithCards(baseCards, upgradeDelta);
WithEnergy(baseEnergy, upgradeDelta);
WithPower<TPower>(baseVal, upgradeDelta, showTooltip);
WithEnchantmentTip<TEnchantment>(amount);
WithCostUpgradeBy(-1);
WithTags(CardTag.Strike); // 或 CardTag.Defend
WithKeywords(CardKeyword.Exhaust, CardKeyword.Retain);
WithKeyword(CardKeyword.Retain, UpgradeType.Add);
WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
WithKeyword(CardKeyword.Ethereal, UpgradeType.Remove);
WithKeyword(CardKeyword.Innate, UpgradeType.Add);
WithTip(typeof(SomePowerOrCard));
// 升级版生成卡牌悬浮提示（参考 WatcherCode Collect.cs）:
// WithTip(new TooltipSource(_ => { var c = ModelDb.Card<Soul>().ToMutable(); c.UpgradeInternal(); return HoverTipFactory.FromCard(c); }));
```

- **X 费卡**（参考 `Collect.cs`）：`base(0, ...)` + `protected override bool HasEnergyCostX => true;` + `var x = ResolveEnergyXValue();`
- **自身复制入牌堆**（参考 `SpellNode.cs`）：`CreateClone()` → `CardPileCmd.AddGeneratedCardToCombat(..., owner, pos)` → `CardCmd.PreviewCardPileAdd([...])`

## 3. 本地化 JSON 模板（`zhs/` 主文件 + 同步复制到 `eng/`）
```json
// cards.json
{
  "SHOREMAGICIAN-EXAMPLE_CARD.title": "示例卡牌",
  "SHOREMAGICIAN-EXAMPLE_CARD.description": "造成{Damage:diff()}点伤害。"
}

// powers.json
{
  "SHOREMAGICIAN-EXAMPLE_POWER.title": "示例能力",
  "SHOREMAGICIAN-EXAMPLE_POWER.description": "效果增加[blue]X[/blue]点。",
  "SHOREMAGICIAN-EXAMPLE_POWER.smartDescription": "效果增加[blue]{Amount}[/blue]点。"
}
```

## 4. `v0.113.1` 破坏性变更与实踩坑点检查表
- `AfterDeath` 含 `CardModel? source` 参数；`HoverTip` 属性只读须用 `HoverTipFactory.*`。
- `STS001` 分析器要求 `zhs/` 与 `eng/` 同步完整；`POOR_SLEEP` 官方中文为「睡眠不佳」。
- `SlowPower` 直接施加原版即可，禁止再自建回合移除 Power 规避并不存在的消失问题。
- `MoonOfLighthousePower` 变化诅咒为 Token/Status 时必须用 `cardScope.CreateCard(...)` 绕过 `CreateRandomCardForTransform` 的默认过滤。
