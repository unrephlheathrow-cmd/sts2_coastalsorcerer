---
name: sts2-shoremagician-dev
description: >-
  Architecture standard and implementation runbook for the Slay the Spire 2
  Coastal Sorcerer (海岸术士 / internal ID ShoreMagician) character mod (BaseLib on STS2 v0.113.1). Use this skill when
  creating, modifying, localizing, or testing Coastal Sorcerer cards, powers,
  relics, commands, hooks, or patches.
---

# Coastal Sorcerer（海岸术士 / 内部 ID `ShoreMagician`）Mod 开发执行指南（BaseLib / STS2 v0.113.1）

本 Skill 为 **Coastal Sorcerer（海岸术士）** 角色 Mod（内部 ID 沿用 `ShoreMagician`）的强制开发规范（对应文本总纲 [开发架构标准.md](../../../开发架构标准.md)）。执行任何编码任务前，严格遵循以下约束，**严禁猜测未验证的 API**。

## 一、不可违反的硬性规则

1. **仅用 BaseLib，禁止混用 RitsuLib**：
   - 卡牌基类：`ShoreMagicianCardModel : ConstructedCardModel`（加 `[Pool(typeof(ShoreMagicianCardPool))]`）
   - 能力基类：`ShoreMagicianPowerModel : CustomPowerModel`
   - 遗物基类：`ShoreMagicianRelicModel : CustomRelicModel`
   - 禁止出现 `ModCardTemplate`、`[RegisterCard]` 或一代 STS1 类名（`AbstractCard`、`AddToBot`、`AbstractDungeon`）。
2. **附魔与变化体系核心铁律**：
   - **唯一指令入口**：施加附魔（`EnchantCard`）、修改/覆盖附魔（`ReplaceEnchantment`）、清除附魔（`ClearEnchantment`）、变化卡牌（`TransformCard` / `TransformToRareSameType`）**必须**调用 `ShoreMagicianCmd`。
   - **以变化牌为统一度量衡（防重计数）**：附魔、修改附魔、移除附魔统一各自只算作 **1 次变化**。修改现有附魔（如 `聚拢冲击`、`道是无晴`、`变幻打击`）**必须**调用 `ReplaceEnchantment<T>`，禁止先 `ClearEnchantment` 再 `EnchantCard` 导致触发 2 次。
   - **常规排斥二次附魔 vs 改魔覆盖**：常规选牌必须用 `ShoreMagicianCmd.CanEnchantWith<T>(card)`（排斥已有附魔的牌）；仅明确说明“变为某种附魔形态”的效果可调用 `ReplaceEnchantment<T>`。
   - **适用范围与法强**：`Nimble`（灵巧）限带格挡牌、`Sharp` 限攻击牌、**`Adroit`（伶俐）可附魔攻击牌**；法强（`MagicPower` + `TempMagicPower`）与翻倍统一由 `Patches/EnchantmentMagicPowerPatch.cs` 与 `EnchantmentExtensions.GetEffectiveAmount()` 增幅 **`Sharp` / `Nimble` / `Adroit` / `Swift` / `Sown`**（`Glam` 与 `Steady` 无数值，不得纳入法强）。
   - **红色潮汐堆叠准则**：`RedTidePower` 多张叠加增加每次触发的格挡 `Amount`，全池每回合仍固定最多触发 `4` 次。
3. **敌人状态尽量沿用原版，规避 Bug**：
   - **禁止**为敌方负面效果自建回合拆除 Power（如早期的 `StagnantSlowDurationPower`），`SlowPower`（缓慢）等状态直接沿用原版机制与全局行为。
4. **版本基线为 `STS2 v0.113.1`（.NET 9.0 / C# 13）**：
   - 未在 [API 速查与模板](./references/api-cheatsheet.md) 列出的底层 API，必须先查 `SlayTheSpire2ModdingTutorials-master`、`WatcherMod_onlyforreference` 或 `sts2.dll` / `SlayTheSpire2.pck` 核实后再写。

## 二、开发前准备（按需读取）

1. 确认卡牌数值与机制细节：读取根目录 [角色与卡牌设定.md](../../../角色与卡牌设定.md)。
2. 查看总架构与构建命令：读取根目录 [开发架构标准.md](../../../开发架构标准.md)。
3. 查找英文类名、路径、实现状态（已实装 48 张 + 待实装 Phase 3B 11 张 + Phase 3C 24 张正式金卡 + 2 张先古牌）：读取 [卡牌实现映射表](./references/card-implementation-map.md)。
4. 查看已核实的 BaseLib / `sts2.dll` API 签名与标准模板：读取 [API 速查与模板](./references/api-cheatsheet.md)。

## 三、新增/修改卡牌、能力或奖励的标准工作流

1. **查表定位**：在 [card-implementation-map.md](./references/card-implementation-map.md) 确定规范英文类名、子目录与实现批次。
2. **先底座后上层**：依赖新能力或特殊钩子（如 `AfterCardChangedPiles`、`AfterShuffle`、`ModifyCardPlayResultLocation`、`RemoveFromCombat`、先古 `ITranscendenceCard`）的先编写底座再写卡牌。
3. **编写 C# 类**：
   - 卡牌统一使用 `ConstructedCardModel` 链式 API；`AfterCardChangedPiles` 监听进手牌（用于 **收束**）、`CardPileCmd.Add(..., PileType.Draw, CardPilePosition.Top)`（用于 **持久战**）、`CardPileCmd.RemoveFromCombat`（用于 **回转+**）、`HasEnergyCostX` + `ResolveEnergyXValue()`（用于 **法术灌注** / **热泉**）。
4. **同步中英本地化 JSON**：
   - 主文件写在 `ShoreMagician/localization/zhs/*.json`，**改完必须同步镜像到 `ShoreMagician/localization/eng/*.json`**。
   - 严禁写死变动数值；`powers.json` 须同时包含 `.title`、`.description`（`X`）与 `.smartDescription`（`{Amount}`）。
5. **编译与打包验证**：
   - 执行 `dotnet build -c Release --no-restore` 确保 `0` 警告 `0` 错误；如有资源/JSON 变更，执行 Godot 4.5.1 headless `--import --quit` 与 `build/pack_mod.gd` 更新游戏目录 `.pck`。
