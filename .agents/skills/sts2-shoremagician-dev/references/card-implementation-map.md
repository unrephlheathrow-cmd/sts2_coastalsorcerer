# ShoreMagician 全量卡牌与组件实现映射表

> ID 前缀规则：BaseLib 自动将 `ShoreMagician.Code.*` 下的类映射为 `SHOREMAGICIAN-<UPPER_SNAKE_CASE>`。
> **当前实装状态**：底座与 **48 张卡牌 + 17 个能力 + 1 遗物 + 1 战后奖励** 已实装并打包；第 5 页 **11 张蓝卡技能（Phase 3B）**、**24 张待实现正式金卡（Phase 3C：7 攻 + 7 技 + 10 能）** 与 **2 张先古卡牌** 已定稿规范类名待开发。

## 1. 底座与核心系统（均已实装）

| 组件 | 文件路径 | 核心职责 |
|---|---|---|
| 入口 | `ShoreMagicianMainFile.cs` | 注册程序集、`ModPatcher`、`ShoreMagicianSubscriber` |
| 角色与池 | `Character/ShoreMagician.cs` 等 4 文件 | `PlaceholderCharacterModel`（HP 75 / Gold 99）、初始 12 张牌、初始遗物 `EnchantedBuckler` |
| 战斗单例 | `Core/ShoreMagicianModel.cs` | `CustomSingletonModel(HookType.Combat)` + `SpireField<PlayerCombatState, TurnTracker>` |
| 指令入口 | `Commands/ShoreMagicianCmd.cs` | `CanEnchantWith<T>`（排斥二次附魔）、`EnchantCard<T>`、`ReplaceEnchantment<T>`（改魔只计 1 次变化）、`ClearEnchantment`、`TransformCard`、`TransformToRareSameType` |
| 内部钩子 | `Events/ShoreMagicianHook.cs` 等 | 派发 `IModifyEnchantmentAmount`、`IOnCardEnchanted`、`IOnEnchantmentCleared`、`IOnCardTransformed` |
| 法强与机制 Patch | `Patches/EnchantmentMagicPowerPatch.cs` | 条件 Strike Tag、排斥二次附魔、**营养汤 (`NutritiousSoup`) 放行 `ManaStrike`**、`MoonOfLighthousePower` 诅咒拦截、`Sharp`/`Nimble`/`Adroit`/`Swift`/`Sown` 法强与翻倍显示/结算、`CardModel.OnPlayWrapper` 翻倍消耗 |
| 法强能力 | `Powers/MagicPower.cs`、`Powers/TempMagicPower.cs` | 永久/临时魔力强度，由 `player.GetMagicPowerAmount()` 读取 |
| 战后奖励 | `Rewards/EnchantAdroitReward.cs` | 战后为牌组 1 张未附魔牌全局附魔 `Adroit(2)`（升级版 `经验汇总+` 为 `Adroit(3)`） |
| 海风诅咒 | `Cards/Curse/SeaBreeze.cs` | `-1` 费不可打出诅咒（`MaxUpgradeLevel => 0`），回合末在手牌失 1 血 |
| 初始遗物 | `Relics/EnchantedBuckler.cs` | 每回合首次打出附魔牌时获得 4 格挡 |

## 2. 基础牌（`Cards/Basic/`，已实装 4 张）

| 中文名 | 类名 | 本地化 Key | 关键实现 |
|---|---|---|---|
| 打击 | `StrikeShoreMagician` | `SHOREMAGICIAN-STRIKE_SHORE_MAGICIAN` | 1 费 6+3 伤，`CardTag.Strike` |
| 防御 | `DefendShoreMagician` | `SHOREMAGICIAN-DEFEND_SHORE_MAGICIAN` | 1 费 5+3 防，`CardTag.Defend` |
| 附加 | `Affix` | `SHOREMAGICIAN-AFFIX` | 1 费技能，选攻击牌调 `EnchantCard<Sharp>(6→8)` |
| 吸纳 | `Absorb` | `SHOREMAGICIAN-ABSORB` | 1 费技能，选任意可附魔手牌调 `EnchantCard<Adroit>(3→4)`（不限技能牌） |

## 3. 白卡（`Cards/Common/`，已实装 11 张）

| 中文名 | 类名 | 费 | 类型 | 关键实现 |
|---|---|---:|---|---|
| 法术防备 | `SpellWard` | 1 | Skill | **`6→8` 防** + 选技能牌 `EnchantCard<Nimble>(3)` |
| 简短咒语 | `BriefIncantation` | 1→0 | Skill | 施加 `DoubleNextEnchantmentPower(1)` |
| 魔导 | `ArcaneGuidance` | 1 | Attack | `7→10` 伤 + 施加 `手牌附魔数+1` 层 `VulnerablePower` |
| 回转 | `Reversion` | 0 | Skill | **不消耗，获 2 费**；敲前清 1 张手牌附魔，**敲后可清附魔或升级（优先附魔）** |
| 溢流 | `Overflow` | 1 | Attack | 全体 `4→5` 伤 ×3 段，置位 `MarkOverflowPlayed` |
| 幻象震慑 | `PhantomShock` | 2 | Skill | `5→7` 防 ×2 段 + 全体 `WeakPower(1)` |
| 蓄能 | `ChargeEnergy` | 1 | Skill | 施加 `TempMagicPower(3)`；敲后 `Retain` |
| 竭尽打击 | `ExhaustingStrike` | 0 | Attack | `CardTag.Strike`，`11→15` 伤 + `ExhaustingStrikeCostPower`（其他牌加 1 费） |
| 支流 | `Tributary` | 1 | Skill | `6` 防 + 仅选**手牌中**变化 `1→2` 张 |
| 魔力打击 | `ManaStrike` | 1 | Attack | `10→13` 伤，手牌每有 1 张**其他附魔牌**额外 `+4`；附魔/溢流/**营养汤**时视作 Strike |
| 凝滞的飞弹 | `StagnantMissile` | 2 | Attack | 施加**原版全局 `SlowPower(1)`**，随后造成 `15→17` 伤 |

## 4. 蓝卡（`Cards/Uncommon/`，共 39 张：28 张已实装 + 11 张待实装）

### 4.1 攻击（已实装 8 张）
| 中文名 | 类名 | 费 | 关键实现 |
|---|---|---:|---|
| 聚拢冲击 | `ConvergeImpact` | 1 | `5×2 伤` → 清手牌至多 4 张附魔 → `ReplaceEnchantment<Sharp>(清除数 × 6/8)` |
| 地动 | `Quake` | 3 | 全体 `21→26` 伤；破盾调用 `CreatureCmd.Stun(enemy)` |
| 升华提取 | `SublimationExtract` | 1 | `15` 伤 + 清 1 牌升级或附魔，施加 `FreeEnchantedCardsPower(1→2)` |
| 解脱 | `Liberation` | 1 | `13→18` 伤；目标每有 1 种负面，本牌 `Sharp` 效应 +100% |
| 魔刺陷阱 | `ThornTrap` | 2 | `18→21` 伤；从消耗堆选 1 张附魔牌打出 |
| 会心一手 | `Masterstroke` | 2 | `14→19` 伤；本回合变化/附魔出攻击牌则回 2 费并亮金光 |
| 虹光尾迹 | `PrismTrail` | 1 | `Exhaust`，`10→12` 伤，抽 `2→3` 后 `TransformToRareSameType` 变化 2 张手牌 |
| 浪潮 | `Surge` | 1 | `Exhaust`（敲后去消耗），全体 `8×2` 伤，手牌攻击牌附随机 1~5 `Sharp` |

### 4.2 技能 · 第 4/2 页（已实装 12 张）
| 中文名 | 类名 | 费 | 关键实现 |
|---|---|---:|---|
| 浏览 | `Browse` | 1 | 抽 `3→4`，弃全部无附魔手牌，每张得 `2→3` 防 |
| 触发 | `Trigger` | 1 | 仅从**手牌中**随机打出无附魔技能牌并附 `Adroit(3)`；敲后优先高费 |
| 静水流深 | `StillWaterGrace` | 1 | `8→10` 防 + `RetainEnchantedHandPower`；敲后 `Retain` |
| 开源节流 | `Temperance` | 1 | 抽 3 + `DrawCardsNextTurnPower(2)` + `NoDrawPower(1)`；敲后 `Retain`（原名节制，避让原版重名） |
| 汲水 | `DrawWater` | 0 | `Exhaust`（敲后移除），回 3 费 + 自施 `WeakPower(1)` + `VulnerablePower(1)` |
| 暮光护身 | `TwilightWard` | 2 | `16→18` 防，对攻击意图敌人施 `WeakPower(2)` + 原版 `SlowPower(1)` |
| 疏浚 | `Dredge` | 1→0 | `Exhaust`，`TransformCard` 至多 3 手牌 + 抽 2 张 |
| 加速 | `Accelerate` | 1 | `8→11` 防 + 选 1 牌 `EnchantCard<Swift>(2)` |
| 法术节点 | `SpellNode` | 0 | `3` 防（敲后 `Retain`），往抽/弃牌堆各加 1 张 `CreateClone()` 复制品 |
| 醒神 | `Awaken` | 2 | `Exhaust`，消耗手牌所有（敲后可选若干）未附魔牌，每张得 `4→5` 防 |
| 眩目闪光 | `DazzlingFlash` | 1 | 全体 `WeakPower(1→2)` + `MagicPower(2)` |
| 感应 | `Resonance` | 2 | **抽 `5→6` 张** + 为手中至多 3 张攻击牌 `EnchantCard<Sharp>(2)` |

### 4.3 技能 · 第 5 页扩展（11 张，#21~#31，[待实现 Phase 3B]）
| 设定序号 | 中文名 | 规范类名 | 费 | 关键词 / 依赖 | 关键实现 |
|---|---|---|---:|---|---|
| 21 | 谐振 | `ResonantMagic` | 0 | `ResonantMagicPower`、`TempMagicPower` | 打出后本回合每打 1 张附魔牌得 `1→2` 临时法强 |
| 22 | 水镜 | `WaterMirror` | 1 | 跨角色 `CardPool` | `9→12` 防，跨角色技能牌选 3 张入手；敲后全部获得 `Ethereal`（不保留） |
| 23 | 法术灌注 | `SpellInfusion` | X | `Exhaust`、`HasEnergyCostX`、`Sharp` | 选 1 攻击牌清附魔后 `EnchantCard<Sharp>(4*X)`（敲后 `X+1`） |
| 24 | 收束 | `Convergence` | 1 | `AfterCardChangedPiles` | 选抽牌堆 1 牌，**首次进入手牌（含抽/定向拿）**得 2 费并抽 `2→3` 后失效 |
| 25 | 淘洗 | `Panning` | 2→1 | `Exhaust`、`Glam` | 选 1 张手牌 `EnchantCard<Glam>(1)` |
| 26 | 海枯石烂 | `DrySeaStone` | 3→2 | `Steady` | `21` 防；选 2 张手牌 `EnchantCard<Steady>(1)` |
| 27 | 风化 | `Weathering` | 1 | `Exhaust`、`SeaBreeze` | 消耗抽/手牌中所有诅咒与状态牌；**无论有无均**往弃牌堆加 `4→3` 张 `SeaBreeze` |
| 28 | 入梦 | `EnterDream` | 1 | `PoorSleep`、`Soul` | 弃牌堆加 1 `PoorSleep`（「睡眠不佳」），抽牌堆加 3 `Soul`（敲后 `Soul+`） |
| 29 | 道是无晴 | `FickleSky` | **2** | `Sown`、`ReplaceEnchantment` | **`13→15` 防**；**弃牌堆至多 2 牌**经 `ReplaceEnchantment<Sown>(1)` 改为播种（吃法强） |
| 30 | 魔力过载 | `ManaOverload` | 2 | `Void`、`MagicPower` | 抽牌堆加 1 `Void`，得 1 `MagicPower` 与 `12→15` 防 |
| 31 | 海风环流 | `SeaBreezeCirculation` | 0 | `Exhaust`、`SeaBreeze` | 消耗手牌任意张，每消耗 1 `SeaBreeze` 得 1 法强；弃牌堆加消耗总数 `SeaBreeze` |

### 4.4 能力（已实装 8 张）
| 中文名 | 类名 | 费 | 对应 Power 类 | 关键实现 |
|---|---|---:|---|---|
| 逢凶化吉 | `TurnIllToGood` | 2 | `TurnIllToGoodPower` | 敲后 `Innate`；每回合首抽诅咒得 2 `MagicPower` |
| 红色潮汐 | `RedTide` | 1 | `RedTidePower` | 变化/改魔/清魔/附魔时获 `3→4` 防（堆叠增数值，每回合固定上限 4 次） |
| 经验汇总 | `ExperienceSummary` | 1 | `ExperienceSummaryPower` / `...UpgradedPower` | 战后为牌组 1 牌全局附 `Adroit(2)`（**敲后 `Adroit(3)`**） |
| 法术规范 | `SpellDiscipline` | 2→1 | `SpellDisciplinePower` | `Ethereal`；**打出一次性获 2 `MagicPower`** + 每回合多抽 1 牌 |
| 灯塔之月 | `MoonOfLighthouse` | 2→1 | `MoonOfLighthousePower` | 战中变化诅咒转为 `Fuel` / `MinionSacrifice` / `Joy` / `Soul` / `Dazed` |
| 粗韧皮肤 | `CallousedSkin` | 2 | `CallousedSkinPower` / `...UpgradedPower` | 免疫 `SeaBreeze`（敲后每次失血 -1）；消耗诅咒/状态得 1 `TempMagicPower` |
| 先声 | `PreemptiveVoice` | 1→0 | `PreemptiveVoicePower` | 每回合首张打出附魔牌效果翻倍 |
| 洋流 | `OceanCurrent` | **2→1** | `OceanCurrentPower` | 抽诅咒继续抽至满手或非附魔牌 |

## 5. 金卡（`Cards/Rare/`，共 25 张正式金卡；3 张测试金卡已移除）

### 5.1 技能牌（8 张）
| 中文名 | 规范类名 | 状态 | 费 | 关键实现 |
|---|---|---|---:|---|
| 奇异浪花 | `WeirdWave` | **已实装** | 0 | `Exhaust`，抽 `2→3`，本回合同名加 1 费，抽/弃各加 1 复制 |
| 扭曲命运 | `TwistFate` | Phase 3C | 0 | `Exhaust`，清弃牌堆全部附魔并消耗弃牌堆全部诅咒，至少消耗 1 张诅咒时获 `1→2` 层 `IntangiblePower` |
| 茕茕独立 | `StandAlone` | Phase 3C | 1 | `Exhaust`（敲后去消耗），`12` 防，弃牌堆 1 牌置抽牌堆底并下次免费 |
| 行船 | `Voyage` | Phase 3C | 1 | 失 `1` `DexterityPower`，获 `2→3` 回合 `RetainHandPower` |
| 湾潮 | `BayTide` | Phase 3C | 0 | 敲后 `Retain`，下回合少抽 3 牌，本回合 `DoubleDamagePower` 伤害翻倍 |
| 洗冤 | `ClearInjustice` | Phase 3C | 1→0 | 本回合每变化/消耗 1 诅咒，抽 1 牌并获 1 `TempMagicPower` |
| 西风海流 | `WesterlyCurrent` | Phase 3C | 1 | 本回合每变化/消耗/附魔 1 牌，全体打 `5→7`（按变化牌度量防重） |
| 遗忘 | `Oblivion` | Phase 3C | 1→0 | 将抽牌堆至多 3 张牌变为 `SeaBreeze` |

### 5.2 攻击牌（7 张 [待实现 Phase 3C]）
| 中文名 | 规范类名 | 费 | 关键实现 |
|---|---|---:|---|
| 飞！弹！ | `FlyBomb` | 2 | `Exhaust`，消耗弃牌堆全部牌；每张附魔牌随机打敌 `21→25`（吃自身攻击 Buff），每张 `SeaBreeze` 全敌永久减 1 力量 |
| 持久战 | `AttritionWar` | 1 | 打 `14→18`，升级全部手牌，并将**持久战自身**放回抽牌堆顶 |
| 局部海啸 | `LocalTsunami` | 0 | `Exhaust`（敲后 `Innate`），全体打 `15`，然后 `CreatureCmd.Stun` 当前生命值 `< 20` 者 |
| 暗流打击 | `UndercurrentStrike` | 2 | `CardTag.Strike`，打 `25→30`，本牌附魔数值翻倍（视为变化）；敲后本场战斗首次打出减 1 费 |
| 热泉 | `HotSpring` | X | `Exhaust`、`HasEnergyCostX`，移除自身全部 `WeakPower` 后全体造成 `X` 次 `13→17` 伤 |
| 下咒 | `CastHex` | 2 | `Ethereal`（敲后去虚无），单体打 `18`，给予 3 虚弱、3 易伤、原版 `SlowPower(3)` |
| 变幻打击 | `MorphStrike` | 1 | `CardTag.Strike`，打 `15→18`，抽 `2`，变化至多 2 手牌 |

### 5.3 能力牌（已录齐 10 / 10 张 [待实现 Phase 3C]）
| 中文名 | 规范类名 | 对应 Power 类 | 费 | 关键实现 |
|---|---|---|---:|---|
| 百川归海 | `AllRiversToSea` | `AllRiversToSeaPower` | 2 | 变化牌时：白→蓝、蓝→金、金→`TarotCards.All` 或其他角色 0 费金卡（敲后给其他角色金卡附加 `Ethereal`） |
| 万象 | `MyriadPhenomena` | `MyriadPhenomenaPower` | 1 | `Ethereal`（敲后去除）；每回合多抽 1 牌，每回合打出的第 1 张牌放至抽牌堆底 |
| 法术痴迷 | `SpellObsession` | — | 1 | `Ethereal`；降低 `2→1` 力量与敏捷，获 3 `MagicPower` |
| 卧虎藏龙 | `CrouchingTiger` | `CrouchingTigerPower` | 2 | 敲后 `Retain`；有 `SlowPower` 的敌人受易伤与虚弱影响翻倍 |
| 大浪淘沙 | `GreatWavesWashSand` | `GreatWavesWashSandPower` | 1 | 每回合首次产生 `SeaBreeze` 获 1 费；敲后可花 1 费打出并消耗 `SeaBreeze` |
| 涌泉相报 | `FountainRepay` | `FountainRepayPower` / `FountainRepayRewardPower` | 0 | `Ethereal`（敲后去除）；下回合与下下回合各自耗能 `<= 3` 后转为 `FountainRepayRewardPower` 战后附 `PerfectFit` |
| 潇湘 | `Xiaoxiang` | `XiaoxiangPower` | 1→0 | 每回合多 1 费；将 1 张 `Cards.Enthralled`（「执迷」）放入抽牌堆底 |
| 漩涡形态 | `VortexForm` | `VortexFormPower` | 3 | 敲后 `Retain`；弃牌堆任意入手 + 消耗抽牌堆至多 3 张；常规打出手牌回抽牌堆底 |
| 飘转 | `DriftTurn` | — | 2 | 经 `ReplaceEnchantment<Momentum>` 将手牌 1 攻击牌改为 `Momentum(8→10)` 并减 1 费 |
| 东风 | `EastWind` | `EastWindPower` | 0 | 敲后 `Innate`；抽 1 牌；洗牌（`AfterShuffle`）对全敌施 1 虚弱 + 1 易伤 |

## 6. 先古卡牌（`Cards/Ancient/`，2 张 [待实现 Phase 3C]）

| 中文名 | 规范类名 | 类型 / 费 | 来源遗物 | 关键实现 |
|---|---|---:|---|---|
| 堤防 | `Levee` | Skill / 1 | `Relics.ArchaicTooth` | 由 `Absorb : ITranscendenceCard` 转换；敲后 `Innate`；手牌任意张附 `Adroit(4)` |
| 无定河 | `WudingRiver` | Power / 1 | `Relics.DustyTome` | `Innate`；解除排斥允许再附魔且同类叠数值；变牌必带随机附魔；敲后变牌已升级 |
