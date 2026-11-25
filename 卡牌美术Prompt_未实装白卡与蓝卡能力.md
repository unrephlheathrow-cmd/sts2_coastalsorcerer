# Coastal Sorcerer（海岸术士）未实装蓝卡能力与白卡 Prompt 合集

> **蓝卡能力（第一节）已全面升级至 V3 项目锚定标准**（遵循 [卡牌与美术设计标准_Prompt规范.md](./卡牌与美术设计标准_Prompt规范.md) V3，以 `anchor_browse.jpg` 与 `anchor_overflow.jpg` 为风格锚点）：
> 1. **渲染与线条**：干净的 2D 平涂赛璐珞、2~3 阶硬边色块、细深色外轮廓、内部几乎无线，无笔刷/纹理/渐变喷枪。
> 2. **角色固定固有色**：深海军蓝帽与斗篷（浅灰锯齿滚边）、浅灰层叠围巾、黑洞脸 + 发光青眼、蓝灰手 + 灰绷带、木杖 + 黄铜钩（每张卡保持一致）。
> 3. **背景与配色**：背景为简单平面图形（斜条纹 / 放射爆发 / 漩涡 / 单一大形），单一背景色相 + 亮青色特效 + 最多 1 个辅助色。
> 4. **上传参考图**：每次生成必须上传 `art_refs/style_refs/anchor_browse.jpg` 与 `art_refs/style_refs/anchor_overflow.jpg`（冷色卡把浏览放第一张，暖色/放射卡把溢流放第一张）。
> 5. **进度说明**：蓝卡能力共 8 张，其中 **洋流（`OceanCurrent`）**、**灯塔之月（`MoonOfLighthouse`）**、**先声（`PreemptiveVoice`）**、**红色潮汐（`RedTide`）**、**粗韧皮肤（`CallousedSkin`）**、**法术规范（`SpellDiscipline`）**、**逢凶化吉（`TurnIllToGood`）** 已实装；目前仅剩 **经验汇总（`ExperienceSummary`）** 1 张待生成（已按新思路改为“高高举起的附魔技能册子”）。

---

## 一、其余 4 张蓝卡能力（3 张已实装 + 1 张待生成，V3 项目锚定标准）

### 1. 粗韧皮肤（`CallousedSkin` / *Calloused Skin*，蓝卡能力）`[已实装]`
- **机制**：“海风”诅咒无法对你造成伤害（升级后每次失去生命少失 1 点）；每消耗一张诅咒或状态牌，获得临时魔力强度。
- **V3 视觉设计（背面回眸 + 龟壳挡风刃｜配色 `C4 海风`｜背景 `B1 斜条纹`）**：
  - 海岸术士侧背对着镜头、回眸从宽檐帽下斜睨观众，神态从容；斗篷背后稳稳背着一面简洁几何六边形深海大龟壳。
  - 一道苍白薄荷绿的“海风”弯月风刃从右侧劈在龟壳弧面上，毫无损伤地崩碎成几片青色魔力光片弹开。
- **推荐参考图顺序**：`anchor_browse.jpg`（第 1 张） + `anchor_overflow.jpg`（第 2 张）
- **Prompt**：
```text
Match the art style, rendering, outline quality and the character design of the attached reference images exactly.
Slay the Spire style card illustration, clean 2D flat cel-shaded digital art. Bold simple shapes, 2-3 flat tones per surface with hard shadow edges, a thin dark outer outline and almost no interior lines, smooth flat fills with no texture. Expressive cartoon acting and a strong, readable silhouette.
The Coastal Sorcerer: an oversized floppy deep navy wide-brimmed pointed wizard hat with a bent tip and jagged brim, the face is a dark void with two glowing cyan eyes, a thick layered light grey scarf wrapped over the lower face, a deep navy cloak with pale grey jagged trim, pale blue-grey hands with grey bandaged forearms, a crooked wooden staff topped with a brass question-mark-shaped hook.
Chest-up back-three-quarter view: the sorcerer wears a large, chunky, simple hexagonal deep-sea turtle shell strapped across their back and calmly glances back over one shoulder toward the viewer. A sweeping pale mint green crescent wind blade slashes from the right and shatters harmlessly against the turtle shell, popping into a few bright cyan diamond shards.
Effects are bold stylized flat shapes in bright cyan, with pale mint green as the only secondary accent.
Background: three wide flat diagonal bands in shades of dark teal and near-black green.
The character fills about half to two-thirds of the frame, focal point in the upper-center; keep the top-left corner and the bottom corners simple. 4:3 horizontal, full-bleed, no frame, no text.
```

---

### 2. 经验汇总（`ExperienceSummary` / *Compendium*，蓝卡能力）`[重设思路 · 待生成]`
- **机制**：战斗结束时，获得额外附魔奖励（为牌组中一张牌附魔：伶俐 `2→3`）。
- **V3 视觉设计（高举附魔技能册子｜配色 `C3 星紫`｜背景 `B2 放射爆发`）**：
  - 放弃“低头画圈”，改为呼应「溢流」的仰视高举张力与战后得宝仪式感：海岸术士居中低机位出镜，双臂高高举起过头顶，把一本厚实精装的深蓝金边**附魔技能册子**（微斜展示书脊、金角扣与封面上的简洁金色符文徽记）骄傲地托举在画面中上方。
  - 一条亮青色附魔光带缠绕着高举的技能册子旋转发光，册子周身迸出几片亮青与金色碎芒；术士外圈带一圈细暖金描边，与深紫放射光环背景干净剥离。
- **推荐参考图顺序**：`anchor_overflow.jpg`（第 1 张） + `anchor_browse.jpg`（第 2 张）
- **Prompt**：
```text
Match the art style, rendering, outline quality and the character design of the attached reference images exactly.
Slay the Spire style card illustration, clean 2D flat cel-shaded digital art. Bold simple shapes, 2-3 flat tones per surface with hard shadow edges, a thin dark outer outline and almost no interior lines, smooth flat fills with no texture. Expressive cartoon acting and a strong, readable silhouette.
The Coastal Sorcerer: an oversized floppy deep navy wide-brimmed pointed wizard hat with a bent tip and jagged brim, the face is a dark void with two glowing cyan eyes, a thick layered light grey scarf wrapped over the lower face, a deep navy cloak with pale grey jagged trim, pale blue-grey hands with grey bandaged forearms. A thin warm gold outline separates the character from the background.
Low-angle chest-up pose: raising both bandaged arms high overhead in triumph, the sorcerer hoists a thick, bound deep-blue enchanted skill booklet aloft into the upper center, angled slightly to show its spine, gold corner clasps, and a simple glowing golden rune emblem on the cover. A bright cyan enchantment ribbon swirls around the raised booklet, popping a few chunky cyan and gold sparks.
Effects are bold stylized flat shapes in bright cyan, with gold as the only secondary accent.
Background: a radial burst of wide rays with concentric rings in shades of violet and deep indigo purple, centered behind the raised booklet.
The character and raised booklet fill about two-thirds of the frame, focal point in the upper-center; keep the top-left corner and the bottom corners simple. 4:3 horizontal, full-bleed, no frame, no text.
```

---

### 3. 法术规范（`SpellDiscipline` / *Arcane Discipline*，蓝卡能力）`[已实装]`
- **机制**：虚无；打出获得 `2` 点魔力强度；每回合开始时多抽 `1` 张牌。
- **V3 视觉设计（端正竖杖校准 + 秩序卡牌环｜配色 `C3 星紫`｜背景 `B2 放射爆发`）**：
  - 海岸术士居中正身出镜、姿态端正克制，双手握住漂流木黄铜海钩法杖垂直立于身前中轴，宛如校准法术的节拍器。
  - 一道简洁锋利的亮青色几何大圆环环绕在角色身前与法杖交叠，三张金色卡牌顺着圆环轨迹排成等距整齐的队列向前滑行，体现“法术秩序与稳定抽牌”。
- **推荐参考图顺序**：`anchor_overflow.jpg`（第 1 张） + `anchor_browse.jpg`（第 2 张）
- **Prompt**：
```text
Match the art style, rendering, outline quality and the character design of the attached reference images exactly.
Slay the Spire style card illustration, clean 2D flat cel-shaded digital art. Bold simple shapes, 2-3 flat tones per surface with hard shadow edges, a thin dark outer outline and almost no interior lines, smooth flat fills with no texture. Expressive cartoon acting and a strong, readable silhouette.
The Coastal Sorcerer: an oversized floppy deep navy wide-brimmed pointed wizard hat with a bent tip and jagged brim, the face is a dark void with two glowing cyan eyes, a thick layered light grey scarf wrapped over the lower face, a deep navy cloak with pale grey jagged trim, pale blue-grey hands with grey bandaged forearms, a crooked wooden staff topped with a brass question-mark-shaped hook.
Centered symmetrical pose: standing upright with composed posture, the sorcerer holds the hooked staff strictly vertical in front of their chest as a crisp, bold bright cyan circular rune ring orbits around and in front of their body, carrying three glowing golden cards spaced evenly in neat formation along the ring.
Effects are bold stylized flat shapes in bright cyan, with gold as the only secondary accent.
Background: a symmetrical radial burst of wide rays with concentric rings in shades of violet and deep indigo purple.
The character fills about half to two-thirds of the frame, focal point in the upper-center; keep the top-left corner and the bottom corners simple. 4:3 horizontal, full-bleed, no frame, no text.
```

---

### 4. 逢凶化吉（`TurnIllToGood` / *Silver Lining*，蓝卡能力）`[已实装]`
- **机制**：升级后固有；每回合首次抽到诅咒牌时，获得 `2` 点魔力强度。
- **V3 视觉设计（掰开诅咒牌现明珠｜配色 `C5 熔金`｜背景 `B3 漩涡`）**：
  - 海岸术士胸以上近景，帽檐下两只发光青眼微微弯起、神态胸有成竹（暖色背景加一圈细暖黄外描边，同「溢流」将角色干净剥离）。
  - 术士双手在胸前像撬开蚌壳般把一张暗紫带刺的诅咒牌生生掰成两半，裂缝正中升起一颗饱满耀眼的亮青色魔力明珠，把两瓣暗紫诅咒壳向外弹开并散出几片青白光屑。
- **推荐参考图顺序**：`anchor_overflow.jpg`（第 1 张） + `anchor_browse.jpg`（第 2 张）
- **Prompt**：
```text
Match the art style, rendering, outline quality and the character design of the attached reference images exactly.
Slay the Spire style card illustration, clean 2D flat cel-shaded digital art. Bold simple shapes, 2-3 flat tones per surface with hard shadow edges, a thin dark outer outline and almost no interior lines, smooth flat fills with no texture. Expressive cartoon acting and a strong, readable silhouette.
The Coastal Sorcerer: an oversized floppy deep navy wide-brimmed pointed wizard hat with a bent tip and jagged brim, the face is a dark void with two glowing cyan eyes, a thick layered light grey scarf wrapped over the lower face, a deep navy cloak with pale grey jagged trim, pale blue-grey hands with grey bandaged forearms. A thin warm yellow outline separates the character from the background.
Chest-up close-up with eyes happily narrowed: holding a dark thorny purple-magenta cursed card in front of their chest like a clam shell, the sorcerer cracks it cleanly in half with both hands as a large, round, blazing bright cyan mana pearl rises out of the split center, pushing the two dark card halves apart with a few chunky cyan shards.
Effects are bold stylized flat shapes in bright cyan, with deep purple-magenta as the only secondary accent.
Background: a few wide flat swirling spiral bands in shades of golden orange and warm brown-red.
The character fills about half to two-thirds of the frame, focal point in the upper-center; keep the top-left corner and the bottom corners simple. 4:3 horizontal, full-bleed, no frame, no text.
```

---

## 二、未实装白卡（9 张）+ 初始卡（1 张）（旧 V1 规范草稿，待按需升级至 V3）

> 当前白卡共 17 张：已实装 8 张（`魔导`、`蓄能`、`眩目闪光`、`竭尽打击`、`溢流`、`幻象震慑`、`回转`、`支流`）；**未实装白卡共 9 张**（含由蓝卡下调至白卡的 `会心一手`、`升华提取`、`疏浚`）+ **未实装初始卡 1 张（`吸纳`）**。以下全部 10 张均严格补全五段式与 `NO meaningless outlines` 规范，可直接复制生图。

---

### A. 白卡攻击（5 张）

### 1. 魔力打击（`ManaStrike` / *Arcane Strike*，白卡攻击）
- **机制**：1 费｜造成 `10→13` 伤害；手牌中每有一张其他附魔牌，额外造成 `4` 点伤害。
```text
Official Slay the Spire card illustration art style, minimalist 2D vector-like flat cel-shaded digital art.
CRITICAL SHADING & NO-MEANINGLESS-OUTLINES RULE: Use large, unified, contiguous flat color blocks (strictly 2 flat tones per surface: one broad lit plane and one single connected shadow mass). Do NOT generate meaningless outlines! NEVER draw internal black lines, decorative crease lines, or sketchy ink strokes inside bandages, enchanted cards, or slash waves. Keep all internal shading boundaries 100% lineless (pure flat color-block meets pure flat color-block directly like cut-paper vector art), reserving a bold dark outer contour and a blazing golden-yellow outer rim-light outline ONLY for the outermost silhouettes. Flat graphic comic impact effects (solid unshaded cream-white starbursts and sharp cyan lightning streaks).
BACKGROUND & PALETTE: Vibrant high-saturation fiery crimson-red and dark wine-maroon graphic background patterned with bold flat diagonal speed-line bands.
80%+ FILL RATIO & EDGE-BLEEDING ZOOM: Extreme tight 4:3 close-up where the bandaged forearm, brass sea-hook staff, stacked enchanted cards, and colossal crescent slash fill 85% of the entire frame and bleed off the lower-left and upper-right edges, leaving less than 15% visible background.
COMPOSITION & STRICT PHYSICAL LOGIC: From the lower-left corner, the Coastal Sorcerer's pale blue-grey hand wrapped in 2-3 broad lineless grey linen bands swings the curved antique brass sea-hook staff diagonally upward-right in a violent sweeping cleave. STRICT PHYSICAL TETHER & VECTOR: Three crisp navy enchanted cards are fanned tightly against the outer brass hook curve—literally locked onto the metal by glowing neon-purple and electric-cyan energy tethers—channeling flat golden-cyan arcs forward along the swing into a colossal, razor-sharp electric-cyan crescent blade wave and a solid unshaded cream-white comic starburst impact at the upper-right. All shockwaves and sparks blast strictly upward-right along one unified slash vector.
Full-bleed 4:3 horizontal illustration.
Negative Constraints: NO meaningless outlines, NO decorative internal linework, NO black crease lines on bandages, NO disconnected floating cards without physical tether, NO fragmented small shadows, NO soft airbrushing, NO card frame, NO borders, NO text, NO watermarks.
```

---

### 2. 凝滞的飞弹（`StagnantMissile` / *Stasis Bolt*，白卡攻击）
- **机制**：2 费｜给予目标缓慢（Slow），造成 `15→17` 伤害。
```text
Official Slay the Spire card illustration art style, minimalist 2D vector-like flat cel-shaded digital art.
CRITICAL SHADING & NO-MEANINGLESS-OUTLINES RULE: Use large, unified, contiguous flat color blocks (strictly 2 flat tones per surface: one broad lit plane and one single connected shadow mass). Do NOT generate meaningless outlines! NEVER draw internal black lines or sketchy facet strokes inside the crystal, stasis rings, or snail. Keep all internal shading boundaries 100% lineless (pure flat color-block meets pure flat color-block directly like cut-paper vector art), reserving a bold dark outer contour and a bright amber-yellow rim-light outline ONLY for the outermost silhouettes. Flat graphic comic impact effects (solid unshaded cream-white and golden burst shards).
BACKGROUND & PALETTE: Vibrant high-saturation burnt-orange and deep wine-red graphic background patterned with bold flat diagonal speed-line bands.
80%+ FILL RATIO & EDGE-BLEEDING ZOOM: Extreme tight 4:3 macro close-up filling 85% of the canvas across the main diagonal, bleeding off the upper-left and lower-right edges, leaving less than 15% visible background.
COMPOSITION & STRICT PHYSICAL LOGIC (SLAY THE SPIRE VISUAL GAG): Hurtling violently from upper-left toward lower-right is a colossal, geometric-faceted electric-cyan and amber stasis crystal harpoon-bolt launched directly off the pointed tip of an antique brass sea-hook braced in the upper-left corner. Tightly encircling the missile shaft are three flat golden clockwork-ripple rings freezing sharp flat water shards motionless in mid-air. STRICT AERODYNAMIC VISUAL GAG: Clinging firmly with its flat base onto the top facet of the speeding crystal bolt is a small flat-shaded glowing seafoam-cyan snail whose eyestalks and speed streaks stream horizontally BACKWARD toward the upper-left against the forward rush.
Full-bleed 4:3 horizontal illustration.
Negative Constraints: NO meaningless outlines, NO internal black lines inside crystal facets or ripples, NO physically contradictory wind direction, NO fragmented small shadows, NO soft gradients, NO card frame, NO borders, NO text, NO watermarks.
```

---

### 3. 会心一手（`Masterstroke` / *Masterstroke*，白卡攻击）
- **机制**：2 费｜造成 `14→19` 伤害；若本回合内变化出过攻击牌，获得 `2` 点能量。
```text
Official Slay the Spire card illustration art style, minimalist 2D vector-like flat cel-shaded digital art.
CRITICAL SHADING & NO-MEANINGLESS-OUTLINES RULE: Use large, unified, contiguous flat color blocks (strictly 2 flat tones per surface: one broad lit plane and one single connected shadow mass). Do NOT generate meaningless outlines! NEVER draw internal black ink lines, sketchy woodgrain, or fine creases inside the brass hook, grey linen bandages, armor plate, or golden sparks. Keep all internal shading boundaries 100% lineless (pure flat color-block meets pure flat color-block directly like cut-paper vector art), reserving a bold dark outer contour and a blazing pure-gold outer rim-light outline ONLY for the outermost silhouettes. Flat graphic comic impact effects (solid unshaded cream-white and sunny-yellow starburst shards).
BACKGROUND & PALETTE: High-saturation fiery vermilion-orange and deep crimson-maroon graphic background patterned with bold flat diagonal speed-line bands.
80%+ FILL RATIO & EDGE-BLEEDING ZOOM: Extreme tight 4:3 close-up filling 85% of the entire canvas, bleeding off the lower-left and upper-right edges, leaving less than 15% visible background.
COMPOSITION & STRICT PHYSICAL LOGIC (ONE-STRIKE DECAPITATION + ENERGY SURGE): From the lower-left, the Coastal Sorcerer's pale blue-grey hand wrapped in 2-3 broad lineless grey linen bands thrusts the pointed curved tip of the antique brass sea-hook forward in one surgical masterstroke strike directly into the central weak-point seam of a cracked obsidian-slate armor barrier filling the right side. STRICT PHYSICAL & ENERGY LOGIC: At the exact spear-tip contact point in the upper-center, an exploding cream-white comic starburst splits the dark slab cleanly outward while TWO massive flat sunny-yellow diamond energy sparks erupt backward from the impact crack straight down the curved brass hook back into the sorcerer's gloved wrist as golden surge arcs.
Full-bleed 4:3 horizontal illustration.
Negative Constraints: NO meaningless outlines, NO decorative internal ink lines, NO black crease lines on bandages, NO disconnected floating icons, NO fragmented small shadows, NO soft gradients, NO card frame, NO borders, NO text, NO watermarks.
```

---

### 4. 升华提取（`SublimationExtract` / *Sublimate*，白卡攻击）
- **机制**：1 费｜造成 `10` 伤害；清除手牌中一张牌的附魔或升级，你的下 `1→2` 张附魔牌耗能变为 `0`。
```text
Official Slay the Spire card illustration art style, minimalist 2D vector-like flat cel-shaded digital art.
CRITICAL SHADING & NO-MEANINGLESS-OUTLINES RULE: Use large, unified, contiguous flat color blocks (strictly 2 flat tones per surface: one broad lit plane and one single connected shadow mass). Do NOT generate meaningless outlines! NEVER draw internal black ink lines inside the rising steam beams, linen wraps, crystal alembic hook, or rune ribbons. Keep all internal shading boundaries 100% lineless (pure flat color-block meets pure flat color-block directly like cut-paper vector art), reserving a bold dark outer contour and a radiant pure-gold outer rim-light outline ONLY for the outermost silhouettes. Flat graphic comic impact effects (solid unshaded cream-white starburst flash).
BACKGROUND & PALETTE: High-saturation fiery crimson-orange and deep rust-red graphic background patterned with bold flat diagonal bands.
80%+ FILL RATIO & EDGE-BLEEDING ZOOM: Extreme tight 4:3 close-up filling 85% of the canvas and bleeding off the bottom-left and top-right edges, leaving less than 15% visible background.
COMPOSITION & STRICT PHYSICAL LOGIC (ALCHEMICAL EXTRACTION STRIKE): Gripped firmly at the bottom-left by the Coastal Sorcerer's pale blue-grey bandaged hand, a slate-grey card's golden-emerald upgrade crest and neon-purple enchantment seal are stripped cleanly off its surface by the curved antique brass sea-hook sliding directly across the card face. STRICT SINGLE VECTOR PHYSICS: The extracted flat golden-emerald and pure-white radiant essence condenses seamlessly along the curved brass hook tip into ONE razor-sharp, blazing pure-gold and cream-white plasma lance firing diagonally upward-right straight into a dark spiky silhouette edge, detonating a crisp flat starburst impact.
Full-bleed 4:3 horizontal illustration.
Negative Constraints: NO meaningless outlines, NO decorative internal linework, NO glass reflection hatching, NO disconnected floating cards without hand/hook contact, NO fragmented small shadows, NO soft airbrushing, NO card frame, NO borders, NO text, NO watermarks.
```

---

### 5. 报应（`Retribution` / *Retribution*，白卡攻击）
- **机制**：1 费｜造成 `15→18` 伤害；将抽牌堆中的一张牌变化为海风。
```text
Official Slay the Spire card illustration art style, minimalist 2D vector-like flat cel-shaded digital art.
CRITICAL SHADING & NO-MEANINGLESS-OUTLINES RULE: Use large, unified, contiguous flat color blocks (strictly 2 flat tones per surface: one broad lit plane and one single connected shadow mass). Do NOT generate meaningless outlines! NEVER draw internal black lines inside splashes, bandages, curse wind, or armor cracks. Keep all internal shading boundaries 100% lineless (pure flat color-block meets pure flat color-block directly like cut-paper vector art), reserving bold outer contours and a blazing amber-gold rim-light outline ONLY for the outermost silhouettes. Flat graphic comic impact effects (solid unshaded cream-white starburst).
BACKGROUND & PALETTE: Vibrant high-saturation fiery crimson-orange to deep wine-maroon graphic background patterned with bold flat diagonal speed-line bands.
80%+ FILL RATIO & EDGE-BLEEDING ZOOM: Extreme tight 4:3 close-up filling 85% of the canvas and bleeding off the bottom-left and upper-right edges, leaving less than 15% visible background.
COMPOSITION & STRICT PHYSICAL LOGIC (PUNISHING STRIKE + CURSE RECOIL): From the lower-left edge, the Coastal Sorcerer's pale blue-grey hand wrapped in 2-3 broad lineless grey bands swings the antique brass sea-hook staff forward in a brutal downward cleave, smashing directly through a dark target armor plate at the center-right with a blazing cream-white comic starburst. STRICT RETRIBUTION RECOIL PHYSICS: At the exact impact moment, an eerie pale-mint and abyssal-purple "Sea Breeze" curse wind backlash whips directly OUT of the cracked strike zone back along the brass shaft toward a clean glowing slate card tucked securely in the sorcerer's lower-left sleeve cuff, slicing and curling that card's top corner into dark-teal ghostly wind crescents.
Full-bleed 4:3 horizontal illustration.
Negative Constraints: NO meaningless outlines, NO decorative internal linework, NO disconnected floating cards without physical tether, NO fragmented small shadows, NO soft gradients, NO card frame, NO borders, NO text, NO watermarks.
```

---

### B. 白卡技能（4 张）+ 初始技能（1 张）

### 6. 法术防备（`SpellWard` / *Spellward*，白卡技能）
- **机制**：1 费｜获得 `6→8` 格挡；为手牌中的一张技能牌附魔：灵巧 `3`（Nimble）。
```text
Official Slay the Spire card illustration art style, minimalist 2D vector-like flat cel-shaded digital art.
CRITICAL SHADING & NO-MEANINGLESS-OUTLINES RULE: Use large, unified, contiguous flat color blocks (strictly 2 flat tones per surface: one broad lit plane and one single connected shadow mass). Do NOT generate meaningless outlines! NEVER draw internal black lines inside water barriers, linen wraps, or feather-wing runes. Keep all internal shading boundaries 100% lineless (pure flat color-block meets pure flat color-block directly like cut-paper vector art), reserving bold outer contours and a crisp pure-white outer rim-light outline ONLY for the outermost silhouettes. Flat graphic comic deflection sparks (solid unshaded cream-white and bright orange shards).
BACKGROUND & PALETTE: Vibrant high-saturation electric-cyan and deep Prussian ocean-blue graphic background patterned with bold flat diagonal bands.
80%+ FILL RATIO & EDGE-BLEEDING ZOOM: Extreme tight 4:3 close-up filling 85% of the canvas, bleeding off the left, right, and bottom edges, leaving less than 15% visible background.
COMPOSITION & STRICT PHYSICAL LOGIC (SINGLE UNIFIED ACTION): Curving prominently across the right side is a thick, flat-shaded translucent aqua geometric water-shield dome deflecting incoming bright-orange sparks strictly outward to the right. Sheltered directly behind the water dome at the left-center, the Coastal Sorcerer's pale blue-grey hand wrapped in 2-3 broad grey bands holds the antique brass sea-hook and touches its curved tip straight onto the center of a crisp navy skill card gripped in the lower sleeve cuff, plating the card with sleek, glowing emerald-aqua feather-wing runes and outward cream-white comic sparks.
Full-bleed 4:3 horizontal illustration.
Negative Constraints: NO meaningless outlines, NO decorative internal linework, NO black lines inside water shapes, NO full standing figure, NO disconnected floating cards, NO soft airbrushing, NO card frame, NO borders, NO text, NO watermarks.
```

---

### 7. 简短咒语（`BriefIncantation` / *Cantrip*，白卡技能）
- **机制**：`1→0` 费｜你的下一张附魔牌的附魔效果翻倍。
```text
Official Slay the Spire card illustration art style, minimalist 2D vector-like flat cel-shaded digital art.
CRITICAL SHADING & NO-MEANINGLESS-OUTLINES RULE: Use large, unified, contiguous flat color blocks (strictly 2 flat tones per surface: one broad lit plane and one single connected shadow mass). Do NOT generate meaningless outlines! NEVER draw internal black ink crease lines on the hat, scarf, wrist wraps, or diamond frames. Keep all internal shading boundaries 100% lineless (pure flat color-block meets pure flat color-block directly like cut-paper vector art), reserving bold outer contours and a glowing electric-cyan rim-light outline ONLY for the outermost silhouettes. Flat graphic comic impact effects (solid unshaded cream-white starburst flash).
BACKGROUND & PALETTE: Intense deep twilight-indigo and royal-violet graphic background patterned with bold flat diagonal bands.
80%+ FILL RATIO & EDGE-BLEEDING ZOOM: Extreme macro close-up filling 85% of the canvas, bleeding off all four edges, leaving less than 15% visible background.
COMPOSITION & STRICT PHYSICAL LOGIC (FINGER-SNAP TWIN RESONANCE): Filling the upper half in tight Z-axis overlap behind the foreground hand are the Coastal Sorcerer's wide-brimmed deep navy wizard hat, broad lineless grey scarf bands, and two playfully narrowed glowing electric-cyan eyes. In the center foreground, their pale blue-grey bandaged hand snaps thumb and middle finger together. Directly at the thumb-finger friction point ignites a crisp flat cream-white and sunny-yellow starburst that projects TWO massive, identical, twin-stacked glowing electric-cyan and neon-magenta diamond arcane frames expanding radially outward.
Full-bleed 4:3 horizontal illustration.
Negative Constraints: NO meaningless outlines, NO decorative internal crease lines, NO letters or numbers, NO symbols inside rings, NO fragmented small shadows, NO soft airbrushing, NO card frame, NO borders, NO watermarks.
```

---

### 8. 疏浚（`Dredge` / *Scour*，白卡技能）
- **机制**：1 费｜抽 `3→4` 张牌；变化 `3` 张手牌；消耗。
```text
Official Slay the Spire card illustration art style, minimalist 2D vector-like flat cel-shaded digital art.
CRITICAL SHADING & NO-MEANINGLESS-OUTLINES RULE: Use large, unified, contiguous flat color blocks (strictly 2 flat tones per surface: one broad lit plane and one single connected shadow mass). Do NOT generate meaningless outlines! NEVER draw internal black ink lines inside splashing wave ribbons, bandages, sea-mud, or burning ash wisps. Keep all internal shading boundaries 100% lineless (pure flat color-block meets pure flat color-block directly like cut-paper vector art), reserving a bold dark outer contour and a crisp pure-white + electric-cyan rim-light outline ONLY for the outermost silhouettes. Flat graphic comic splash droplets and cream-white spray shards.
BACKGROUND & PALETTE: Vibrant high-saturation electric-cyan and deep Prussian ocean-blue graphic background patterned with bold flat diagonal speed-line bands.
80%+ FILL RATIO & EDGE-BLEEDING ZOOM: Extreme tight 4:3 macro close-up filling 85% of the canvas, bleeding off the lower-left and upper-right edges, leaving less than 15% visible background.
COMPOSITION & STRICT PHYSICAL LOGIC (DREDGING UP FROM THE SEABED + TRANSMUTATION & EXHAUST): Plunging aggressively from the upper-left, the Coastal Sorcerer's pale blue-grey bandaged hand drives the curved antique brass sea-hook deep down into a swirling flat dark-indigo seabed silt vortex at the lower-center, hauling upward a dramatic fan of FOUR glowing slate-blue cards bursting upward out of a violent electric-cyan splash. STRICT TRANSFORMATION & EXHAUST PHYSICS: Three of the hauled cards are visibly warping mid-air at their upper edges along the brass hook's glowing purple-teal swirl—shifting into neon-magenta and seafoam-cyan new forms—while the worn outer wooden dredge-scoop / burnt grey parchment edge at the lower-right cleanly dissolves upward into thin pale-ash geometric exhaust embers.
Full-bleed 4:3 horizontal illustration.
Negative Constraints: NO meaningless outlines, NO decorative internal linework, NO black lines inside water splashes, NO disconnected floating cards outside the hook/water arc, NO fragmented small shadows, NO soft airbrushing, NO card frame, NO borders, NO text, NO watermarks.
```

---

### 9. 信风（`TradeWind` / *Trade Wind*，白卡技能）
- **机制**：`1→0` 费｜所有敌人在本回合失去 `4` 点力量；往手牌中加入 `1` 张海风。
```text
Official Slay the Spire card illustration art style, minimalist 2D vector-like flat cel-shaded digital art.
CRITICAL SHADING & NO-MEANINGLESS-OUTLINES RULE: Use large, unified, contiguous flat color blocks (strictly 2 flat tones per surface: one broad lit plane and one single connected shadow mass). Do NOT generate meaningless outlines! NEVER draw internal black ink lines around wind ribbons, cloth wraps, or weapon shards. Keep all internal shading boundaries 100% lineless (pure flat color-block meets pure flat color-block directly like cut-paper vector art), reserving bold outer contours and a glowing seafoam-cyan rim-light outline ONLY for the outermost silhouettes. Flat graphic wind speed-slashes (solid unshaded icy-white and pale seafoam ribbons).
BACKGROUND & PALETTE: Vibrant high-saturation deep ocean-blue and electric-aqua graphic background patterned with bold flat diagonal gale bands.
80%+ FILL RATIO & EDGE-BLEEDING ZOOM: Extreme tight 4:3 close-up filling 85% of the canvas and bleeding off the left, right, and upper-right edges, leaving less than 15% visible background.
COMPOSITION & STRICT PHYSICAL LOGIC (OVERPOWERING GALE + SEA BREEZE RESIDUE): From the bottom-left, the Coastal Sorcerer's bandaged hand hooks the curved antique brass sea-hook sideways into a rushing, sweeping ribbon of pale seafoam-green and icy-white gale-force trade winds blasting rightward. STRICT PHYSICS: The howling gale slams head-on into two shadowy, heavy dark-maroon clawed gauntlets and spiked maces lunging from the right, instantly knocking them backward in defeat and stripping their fiery strength sparks cleanly away toward the right edge, while a single trailing dark-teal ghost-wind crescent peels straight off the brass hook into a crisp card held securely in the sorcerer's lower hand.
Full-bleed 4:3 horizontal illustration.
Negative Constraints: NO meaningless outlines, NO black outlines around wind streaks, NO internal black crease lines, NO small full-body standing figure, NO fragmented small shadows, NO soft airbrushing, NO card frame, NO borders, NO text, NO watermarks.
```

---

### 10. *(初始技能)* 吸纳（`Absorb` / *Absorb*，初始技能）
- **机制**：1 费｜为手牌中的一张牌附魔：伶俐 `4→6`（Adroit）。
```text
Official Slay the Spire card illustration art style, minimalist 2D vector-like flat cel-shaded digital art.
CRITICAL SHADING & NO-MEANINGLESS-OUTLINES RULE: Use large, unified, contiguous flat color blocks (strictly 2 flat tones per surface: one broad lit plane and one single connected shadow mass). Do NOT generate meaningless outlines! NEVER draw internal black lines around mist streams, card edges, or linen wraps. Keep all internal shading boundaries 100% lineless (pure flat color-block meets pure flat color-block directly like cut-paper vector art), reserving bold outer contours and an electric-cyan rim-light outline ONLY for the outermost silhouettes. Flat graphic comic absorption sparks (solid unshaded cream-white and bright cyan shards).
BACKGROUND & PALETTE: High-saturation deep Prussian-blue and dark ocean-teal graphic background patterned with bold flat diagonal bands.
80%+ FILL RATIO & EDGE-BLEEDING ZOOM: Extreme tight 4:3 close-up filling 85% of the canvas and bleeding off the lower-left and outer edges, leaving less than 15% visible background.
COMPOSITION & STRICT PHYSICAL LOGIC: From the lower-left, the Coastal Sorcerer's pale blue-grey hand wrapped in 2-3 broad lineless grey linen bands grips both the curved antique brass sea-hook AND the bottom edge of a large centered slate-blue card framed inside the brass hook arc. Broad, flat-shaded ribbons of glowing seafoam-cyan and neon-violet sea-mist spiral centripetally INWARD from all four edges straight through the brass hook into the face of the held card, igniting a sharp, sleek flat purple-cyan swift-wing rune emblem at its core.
Full-bleed 4:3 horizontal illustration.
Negative Constraints: NO meaningless outlines, NO decorative internal linework, NO disconnected floating cards without hand contact, NO fragmented small shadows, NO soft airbrushing, NO card frame, NO borders, NO text, NO watermarks.
```
