# 杀戮尖塔 1 & 2 卡面画风解析与 Coastal Sorcerer（海岸术士）美术 Prompt 标准规范

> **定位**：本文档为 **Coastal Sorcerer（海岸术士）** 后续所有卡牌（`500×380` 普通卡 / `500×702` 先古卡）、遗物与事件插画的**强制美术生成标准**。旨在彻底消除 AI 默认的“写实暗黑厚涂 / 灰暗背景 / 呆板全身摆拍”问题，精准还原《杀戮尖塔》（Slay the Spire 1 & 2）标志性的**高饱和戏剧色块、硬边赛璐珞光影、彩色外发光描边与极端张力特写**画风。

---

## 一、《杀戮尖塔》原版卡面画风的四大核心具象特征（为什么之前生成的图不像原版？）

对比原版卡图（如观者的 `Crush Joints`、`Eruption`、`Vigilance`、`Flying Sleeves`、`Blasphemy`、`Carve Reality`）与常规 AI 插画，原版卡面具有以下 **4 个不可妥协的视觉基因**：

### 1. 配色风格：摒弃写实灰暗场景，采用「高饱和戏剧色场 + 功能色彩编码」
- **严禁写实灰暗自然背景**：原版卡图**极少描绘写实的灰暗天空、石墙或远景海滩**。背景一律高度概括为**高饱和度的单色/双色渐变色场（Vibrant High-Saturation Color Field）**，辅以抽象的速度线、波浪条纹、裂纹或能量团块。
- **按卡牌类型强制色彩编码（手牌余光一秒辨识）**：
  | 卡牌类型 / 流派 | 背景主色调（Background Palette） | 前景主光效 / 撞色（Foreground Accent） | 原版对标参考 |
  |---|---|---|---|
  | **攻击牌 (Attack)** | **炽烈亮橙 `#FF6A00` → 朱红 `#D91E18` / 深酒红 `#4A0808`** 高对比暖色背景 | **电光青蓝 `#45F3FF` / 亮明黄 `#FFE600`** 刀光弧线或白热受击星芒（冷暖强烈撞色） | `Crush Joints`、`Flying Sleeves`、`Reach Heaven` |
  | **技能 / 格挡牌 (Skill / Block)** | **亮蔚蓝 `#00A8FF` / 电光青 `#00E5FF` → 深海蓝 `#072A5E`** 澄澈冷色背景（带水平/弧形水波纹色带） | **纯白 `#FFFFFF` + 亮青/翠绿** 几何切割护盾流光、锐利折线波纹 | `Vigilance`、`Sanctity` |
  | **能力牌 (Power)** | **深星空紫 `#1D0B46` / 皇家靛蓝 → 霓虹洋红 `#FF2A85` / 炫金 `#FFB800`** 辐射状能量背景 | 角色全身包裹**双层亮色外发光轮廓环（纯白内环 + 霓虹青/金/紫粗外环）** | `Blasphemy`、`Eruption`、`Fasting` |
  | **诅咒 / 海风流派 (Curse / Sea Breeze)** | **深渊墨青 `#081C20` → 幽灵苍绿 `#5CE6B8` / 惨白迷雾** 的压抑漩涡色场 | **暗紫黑撕裂暗影条带** + 惨白/幽绿气流细丝 | `Normality`、`Writhe` |

### 2. 光影与笔触设计：「大面积完整色块拼接 + 内部去黑线 + 彩色发光外轮廓」
- **核心铁律 —— 阴影必须是“完整连贯的大色块”，严禁细碎衣褶与美漫黑线排线（Macro Color Shapes, NO Fragmented Micro-Shading）**：
  - **错误示范（AI 常见通病）**：像美漫或《暗黑地牢》那样在围巾、绷带、袖子、斗篷上画满细碎的黑色折痕线、零散的小阴影斑块、短排线（`micro-folds, fragmented shadow islands, heavy internal black ink creases, comic hatching`）。
  - **原版正确画法**：
    1. **大块面极简概括（Large Contiguous Flat Shapes）**：每个部位（如帽子、围巾、袖子、斗篷、手掌）只拆分为 **1 块完整的受光主色块 + 1 块完整连贯的大面积暗部色块**（最多不超过 2~3 个大色阶），绝不把阴影打碎成几十个小碎片。
    2. **外粗内无（Bold Outer Contour, Lineless Internal Shading）**：**仅在角色/物体最外圈保留粗轮廓线 + 亮色外发光描边**；而物体**内部的明暗交界线直接由两个纯色块硬边拼接（不加黑色内部勾线分割）**，衣褶与腕部绷带用宽大简练的色块条带概括，保持画面像矢量剪贴画一样整洁成片。**严禁生成无意义的描边（NO Meaningless Outlines）**：绝不允许在物体内部的色块、锈迹、水纹、泡沫、金属接缝或明暗交界处添加任何装饰性黑线或无意义的细碎描边！内部结构必须 100% 由纯色块与纯色块硬边直接拼接（如同丝网印刷/剪纸矢量色块）。
    3. **扁平化漫画冲击特效（Flat Graphic Impact FX）**：攻击光束、挥砍残影与受击爆炸星芒采用**内部零渐变的纯平涂亮色/米白色大几何块（Flat cream-white / cyan jagged comic starbursts and tapered beams with clean dark outlines）**，参考原版铁甲战士/静默猎手基础防御卡的极简平涂白光星芒。
- **彩色发光外轮廓线（Crisp Colored Outer Rim Outline）**：
  - 沿着角色、武器、手部的最外侧轮廓，包裹一圈**干净锐利的高对比亮色外描边**（如深海青蓝背景配**亮青色 `#45F3FF` 外描边**，暖橙红背景配**亮明黄/鲜橙外描边**），将主体从分层斜向平涂色带背景中干净剥离。
- **反光设计（Broad Geometric Cut-Out Speculars）**：
  - 黄铜法杖钩头、刀刃、护盾的反光画成**宽大、连贯的纯白或亮黄几何长条色块**，不画细碎高光点。
- **影子的形态（Unified Graphic Shadow Masses）**：
  - 阴影必须合并成**一整片连通的大几何暗面（Unified connected shadow mass）**，边缘平滑或带大折角，暗部填入纯净的深靛蓝、深酒红或深墨青，不填脏灰。

### 3. 动作构图与镜头语言：「视觉折叠（Z轴交叠） + 拧身奔跑高张力动势 + 特色法杖轨迹」

> **核心避坑（严禁“左人右特效”站桩平铺）**：
> 绝不允许生成“角色在画面左半边保持原地 3/4 站立立绘姿势、仅仅把法杖伸到右半边放个特效”的平庸构图！这种左右平铺会让画面松散、死板且严重同质化。

必须严格执行以下三大构图与动作铁律：

#### 铁律 3.1 —— 有效面积占比 `>= 80%`、破框出血与「视觉折叠」（80%+ Subject Fill Ratio, Edge-Bleeding Crop & Z-Axis Folding）
- **有效主体面积占比 `>= 80%~85%`（严禁大片空背景）**：
  - **错误示范**：把角色画成画面中央的一个完整小人，导致左右两侧或四周留出大面积空白纯色/速度线背景（背景占比超过 40%）。
  - **原版正确标准**：主体（近景躯干/手部 + 法杖 + 核心特效）必须**占满整个画幅的 `80%~85%` 以上**，**背景裸露面积严格控制在 `15%~20%` 以内**（仅在手臂、武器与特效的夹缝间露出高饱和背景色块）。
  - **强制破框裁切（Edge-Bleeding / Cropped by Frame Borders）**：镜头必须极其贴近（Extreme tight zoom），让角色的披风、肩膀、手肘或巨型法杖挥砍轨迹**直接撑满并延伸出画框的左、右、上/下边缘（被画框边缘自然截断）**，绝不把人物完整收在画框内部。
- **什么是“Z 轴视觉折叠”**：把【前景特效/受击面】、【武器/手部】、【压低的帽檐/扭转的躯干】、【后景飞扬的披风】在**前后纵深（Z轴）上进行多层交叠压缩**，让肢体、法杖与特效在画面中央紧密咬合、互相遮挡穿插。

#### 铁律 3.2 —— 严密物理因果与「单一力场逻辑」（Strict Physical Logic & Unified Force Vector）
- **碎屑、流体与衣物的物理受力一致性**：
  - 每一张卡面都是**单一力学爆发瞬间**。画面中所有的碎屑、火花、水滴、披风下摆、帽檐形变，**必须 100% 服从同一个物理力场方向**：
    - **重击/爆炸类**：所有岩石碎块、火花、气浪必须从**法杖撞击中心点沿着冲击波向外、向上呈放射状高速飞溅（Fly radially OUTWARD away from the point of impact）**，披风受反冲气浪向上掀飞；
    - **挥砍/旋转类**：水花与符文残影必须严格沿着**法杖运动弧线的切线方向（Tangential to the swing arc）**向外甩出；
    - **牵引/吸收类**：能量丝带必须有**明确的物理起点与汇入终点**（如从左手的卡牌表面被扯出、直接吸入右手捏着的法杖钩环内）。
- **严禁一图双动作 / 严禁悬空堆砌“意义不明的象征物”（Single Unified Action, NO Split Tasks or Disconnected Props）**：
  - **单一动作铁律**：一张卡面**只能表现 1 个高度统一的物理动作**（严禁“左手举盾挡左边、右手同时画符搞右边”的双线分心构图）；若卡牌包含双重机制（如「法术防备」= 格挡 + 灵巧附魔），必须通过**单一动作的视觉结果**自然融合（如给一面带有盾牌徽章的技能卡镀上翠青护盾附魔层），而非拆成两个动作。
  - 绝不允许在背景空处凭空画几张毫无物理接触、方向与主冲击波相悖的“掉落卡牌”或孤立图标（如主动作在向下砸地向外爆破，右上角却凭空画三张垂直往下掉的灰牌）。
  - **所有出现的卡牌、道具或特效，必须与角色的手指、法杖钩头或冲击波产生直接的物理接触与力学因果关系**！

#### 铁律 3.2 —— 极限肢体张力与非常规法杖轨迹（Torso Torsion, Sprinting/Leaping & Exotic Staff Trajectories）
- **肢体动势库（每次必选其一，严禁直立站桩）**：
  1. **拧腰回旋（Corkscrew Torso Twist / Spinning Whip-Turn）**：下半身与上半身呈强烈的反向扭转，披风像螺旋桨一样甩开，脖颈与帽檐压到极低；
  2. **疾奔突进 / 贴地滑铲（Low Sprinting Lunge / Ground-Skimming Dash）**：身体前倾近乎平行于地面疾冲，单手在身后拖曳法杖或双手向前猛刺；
  3. **腾空后仰 / 跃起下劈（Mid-Air Backflip / Overhead Leaping Slam）**：双脚离地或身体大幅度后仰蓄力，形成紧绷的满弓张力曲线（Line of Action）。
- **特殊法杖挥舞轨迹（拒绝死板的侧面小月牙）**：
  1. **环绕全身的 S 型 / 8 字螺旋轨迹（Wrap-Around S-Curve / Figure-8 Spiral Trajectory）**：法杖轨迹从画面后方绕过角色肩膀/头顶再猛然劈向镜头正前方，光带将角色身体半包围折叠在其中；
  2. **冲向镜头的极限透视劈刺（Extreme Foreshortened Thrust / Whip-Arc Towards Camera）**：黄铜海钩头就在镜头最前景（巨大化），杖身向后连接角色紧绷的手臂；
  3. **反手回旋挑击 / 双手交叉架杖（Reverse-Grip Uppercut / Cross-Armed Staff Lock）**：双臂在胸前交叉锁死或反手由下往上掀起巨浪。

#### 铁律 3.3 —— 同质化卡牌的「玩梗 / 双关冷幽默 / 脑洞彩蛋」（Slay the Spire Visual Gags & Puns）
- 延续《杀戮尖塔》原版“卡名双关与冷幽默”（如 `Spite` 恶意 → 紧握一罐 `Sprite` 雪碧汽水）的设计传统，对功能相近或名称自带梗点的技能/状态/过牌卡，采用**巧妙的字面双关、现代/海洋反差梗或冷幽默构图**，彻底拒绝“每张卡都是挥法杖”：
  | 卡牌名称 | 正常功能 | 推荐玩梗 / 趣味视觉设计方向（供生图选用） |
  |---|---|---|
  | **浏览 (`Browse`)** | 抽牌并弃无附魔牌得格挡 | **“网页浏览器 / 书摊白嫖梗”**：海岸术士一只手撑着下巴，面前悬浮着三块发光的魔法水镜窗口（左上角甚至有极简的 `×` 关闭符文按钮），正用手指漫不经心地像刷手机/划掉垃圾弹窗一样把几张普通白板卡“左滑丢进垃圾桶” |
  | **开源节流 (`Temperance`)** | 抽 3 张，本回合禁抽、下回合多抽 | **“拧水龙头梗”**：海岸术士用黄铜法杖钩头当扳手，使出吃奶的劲把一根巨大的黄铜魔法水管阀门死死拧紧，阀门后方鼓起一个快要撑爆的巨大发光蓝色水球（囤到下回合爆发） |
  | **经验汇总 (`ExperienceSummary`)** | 战后给牌组 1 张牌永久附魔伶俐 | **“深夜加班写 PPT / 年终总结梗”**：海岸术士戴着巫师帽趴在礁石桌前，面前堆着一摞发光的战斗报告羊皮纸，一手抓着狂掉头发的巫师帽、一手用羽毛笔在一张卡牌上疯狂盖下“合格/伶俐”印章 |
  | **汲水 (`DrawWater`)** | 0 费获 3 费，自施 1 虚弱 1 易伤 | **“字面画水（Draw Water） / 累瘫打水梗”**：双关英文 `Draw Water`（既可指“打水”也可指“画水”）——术士拿着法杖当画笔在半空画出一个巨大的简体“水”字/水滴涂鸦；或者从一口深井里吃力地拽出一桶装满发光能量球的重水，累得闪了腰（对应自施虚弱易伤） |
  | **醒神 (`Awaken`)** | 消耗未附魔手牌得格挡 | **“冰美式 / 泼冷水提神梗”**：海岸术士刚睡醒般帽檐歪斜，一团长着拳头形状的巨型冰冷海浪迎面直接拍在自己脸上（或手里握着一杯冒着极寒蓝光的特浓“深海冰咖啡”），两颗青色眼睛瞬间瞪成感叹号形状 |
  | **飞！弹！ (`FlyBomb`)** | 消耗弃牌堆所有牌随机轰炸 | **“字面飞翔的炸弹 / 扑克牌轰炸机梗”**：弃牌堆的一整副发光附魔卡牌折叠成一架架纸飞机的形状，像蜂群轰炸机一样拖着青色尾焰俯冲轰炸 |
  | **海风环流 (`SeaBreezeCirculation`)** | 消耗手牌海风得法强并再塞回海风 | **“电风扇吹自己 / 莫比乌斯吹风梗”**：几张「海风」诅咒卡在术士身前围成一台高速旋转的三叶电风扇扇叶，狂风把术士的宽檐巫师帽和披风吹得向后水平飞起 90 度 |
  | **持久战 (`AttritionWar`)** | 攻击并升级全部手牌，自身回抽牌堆顶 | **“打铁铺流水线 / 旋转门梗”**：术士一只手抡起法杖当铁锤在铁砧上叮当作响地给一排手牌“敲升级（+）”，自己脚下踩着传送带刚打完一拳又被传送回队伍最前面 |

### 4. 容易忽略但决定成败的「五大隐藏技术参数」
1. **画幅与卡框五边形裁切安全区（`500×380` & Bottom-V Safe Zone）**：
   - 普通卡牌最终分辨率固定为 **`500×380`**（宽高比约 `4:3`，生成时推荐用 `4:3` 横幅后缩放到 `500×380`；先古牌为 `500×702` 竖幅）。
   - **关键避坑**：游戏内卡牌立绘框的**底部左右两侧会被斜切成倒五边形尖底，左上角会被能量费用球遮挡**！因此：**核心视觉焦点（手、脸、法杖钩头、受击爆点）必须严格处在画面中央偏上的钻石安全区内（水平 `15%~85%`、垂直 `12%~78%`）**，底部左右两角（Bottom-left & Bottom-right corners）和左上角（Top-left corner）只放背景色块或延伸的衣摆/流光！
2. **缩略图可读性与元素上限（Thumbnail Readability & 3-Element Cap）**：
   - 手牌区卡图实际显示宽度仅约 `140px`。单张卡面**严禁超过 3 个视觉层级**（① 背景色场/速度线 + ② 核心主体[手/法杖/半身] + ③ 1 个主特效/受击目标）。严禁画密密麻麻的小怪群、复杂的室内家具或细小文字。
3. **禁止出现任何 UI / 文字 / 卡框（Zero Framing / Text Bleed）**：
   - 画面必须是**100% 满幅出血纯插画（Full-bleed illustration edge-to-edge）**，严禁 AI 在画面四周画卡牌边框、圆角黑边、卷轴框或伪造文字。
4. **海岸术士（Coastal Sorcerer）角色视觉一致性锚点（Character Visual Anchors）**：
   - **出现半身/头部时**：`tattered wide-brimmed deep navy wizard hat pulled low, thick layered grey linen cloth face-wrap covering nose and mouth, two glowing pupil-less cyan eyes inside the dark shadow under the hat brim, deep Prussian blue cloak with pale grey jagged trim, small pale scallop shells hanging from brown leather belt`。
   - **仅出现手部/小臂特写时**：`pale cool blue-grey skin hand with tense fingers, layered dark grey linen wrist wraps and Prussian blue sleeve cuff with pale grey trim`。
   - **出现法杖时**：`gnarled dark driftwood staff topped with a curved antique brass sea-hook head`。
5. **原版「附魔 / 变化」专属视觉符号（Mod Visual Motifs）**：
   - **附魔（Enchantment）**：用**锋利的亮青色/亮金色几何符文切面与围绕武器旋转的硬边高光带**表现（而非柔光粒子雾）；
   - **变化（Transform）**：用**卡牌/光影沿着硬朗折线裂开、蜕变为高饱和新色块形态**表现；
   - **海风诅咒（Sea Breeze）**：用**苍白/幽青色的锋利气流弯刃与漩涡暗影条带**表现。

---

## 二、标准模块化 Prompt 模板（中英双语，直接复制组合使用）

今后生成任何卡牌插画时，采用 **「模块 1（画风底座） + 模块 2（卡牌类型配色） + 模块 3（角色/局部锚点） + 模块 4（单卡具体动作与构图） + 模块 5（负面约束）」** 组合生成：

### 模块 1：杀戮尖塔原版画风底座（所有卡牌必带，重点强化“大色块完整性、严禁生成无意义的描边”）
> **English Prompt Core (Macro Flat Color Blocks & Strict No-Meaningless-Outlines Rule)**:
> `Official Slay the Spire card illustration art style, minimalist 2D vector-like flat cel-shaded digital art. CRITICAL SHADING & NO-MEANINGLESS-OUTLINES RULE: Use large, unified, contiguous flat color blocks (strictly 2 tones per surface: 1 broad lit plane and 1 single connected shadow mass). Do NOT generate meaningless outlines! NEVER draw internal black lines, decorative ink strokes, or sketchy contour lines around internal color patches, surface textures, water ripples, rust, or shading boundaries. Keep all internal surfaces 100% lineless (pure flat color-block meets pure flat color-block directly like cut-paper vector art), reserving a bold outer contour and a crisp high-contrast colored outer rim-light outline ONLY for the outermost silhouette of the main subject. Flat graphic comic impact effects (solid unshaded cream-white or cyan starbursts and tapered beams). Full-bleed 4:3 horizontal illustration.`

### 模块 2：按卡牌类型四选一的「背景与光影配色指令」
- **【选项 2A：攻击牌 Attack —— 炽橙绯红背景 + 青金撞色】**
  > `Background & Palette: Vibrant high-saturation fiery orange-to-crimson red dramatic gradient background with bold dark maroon graphic slash streaks and speed lines. Strong warm-vs-cool color contrast between the blazing orange-red background, bright golden-yellow rim outlines, and electric cyan magic slash arcs.`
- **【选项 2B：技能/防御牌 Skill / Block —— 电光青蓝背景 + 纯白锐利流光】**
  > `Background & Palette: Vibrant high-saturation electric cyan to deep ocean blue dramatic graphic background patterned with stylized flat wave bands. Brilliant pure-white and neon-aqua geometric barrier shards and sharp diagonal light streaks, cool crisp high-contrast lighting.`
- **【选项 2C：能力牌 Power —— 星空深紫/炫金洋红能量场 + 双层发光轮廓】**
  > `Background & Palette: Intense royal purple, neon magenta, and radiant gold energy vortex background with bold graphic radiating beams. The subject is silhouetted in high-contrast shadow and wrapped in a thick double-layered glowing white and electric-cyan aura outline.`
- **【选项 2D：诅咒/海风互动牌 Curse / Sea Breeze —— 深渊墨青 + 苍白幽绿风暴】**
  > `Background & Palette: High-contrast abyssal dark teal and eerie spectral seafoam-green swirling background with bold jagged dark-indigo shadow ribbons and razor-sharp pale wind crescents.`

### 模块 3：按出镜范围三选一的「海岸术士角色锚点」
- **【选项 3A：局部手部 + 法杖特写（不露脸，推荐用于 50% 以上的攻击/附魔/法术牌）】**
  > `Subject Anchor (Close-up Hands & Staff): Extreme close-up on the Coastal Sorcerer's pale cool blue-grey hands wrapped in 2-3 broad smooth dark-grey linen wrist bands and Prussian-blue sleeve cuffs with pale-grey jagged trim, gripping a gnarled driftwood staff topped with a curved antique brass sea-hook head.`
- **【选项 3B：高张力半身出镜（露巫师帽、面巾与发光青眼）】**
  > `Subject Anchor (Dynamic Bust): Tight chest-up close-up of the Coastal Sorcerer wearing a tattered wide-brimmed deep navy wizard hat pulled low, simplified broad-band grey cloth face-wrap covering nose and mouth, two glowing pupil-less cyan eyes in the deep shadow under the hat brim, Prussian-blue cloak with pale-grey jagged trim and small scallop shells on the belt, wielding a gnarled staff with a curved antique brass sea-hook head.`
- **【选项 3C：纯魔法造物 / 敌人受击局部特写（主角不出镜）】**
  > `Subject Anchor (Object / Impact Focus): Extreme close-up focusing purely on the magical construct, enchanted weapon, or target impact in the foreground, using the Coastal Sorcerer's signature electric-cyan rune geometry and antique brass/sea-tide motifs.`

### 模块 4：单卡动作构图、「80%+ 满幅破框占比」与「严密物理逻辑」指令（按单卡设计填写）
> `Composition, 80%+ Fill Ratio & Strict Physical Logic: EXTREME TIGHT ZOOM—the main subject (character bust/hands, staff, and primary effect) MUST fill 80%-85% of the entire canvas, bleeding off at least 3 edges of the frame, leaving less than 20% visible background in the gaps. Use tight Z-axis visual folding. [在此填写具体高张力动作与物理因果细节：必须明确规定 ① 唯一受力中心点与碎屑/火花/水花向外飞溅的物理方向；② 所有卡牌/道具与手或法杖的直接物理接触关系，严禁在背景空处悬空画无关联的掉落物].`

### 模块 5：负面排除约束（所有卡牌必带，重点封杀无意义描边、大片空背景与碎阴影）
> `Negative Constraints: NO meaningless outlines, NO decorative internal linework, NO black outlines around water ripples or surface textures, NO blurry glowing bokeh dots, NO large empty background areas, NO small zoomed-out character floating in the middle of the canvas, NO disconnected floating or randomly falling objects/cards with unclear meaning, NO physically contradictory debris directions, NO static standing idle pose, NO side-by-side layout, NO fragmented small shadows, NO micro-folds, NO internal black ink crease lines, NO comic cross-hatching, NO soft gradient airbrushing, NO card frame, NO borders, NO text, NO watermarks.`

---

## 三、三类典型卡牌的完整合成 Prompt 范例（可直接用于生图测试）

### 范例 1：攻击牌（以「魔力打击 Mana Strike」或「解脱 Liberation」为例 —— 局部特写 + 亮橙红背景）
```text
Official Slay the Spire card illustration art style, 2D graphic vector-like cel-shaded digital art, bold posterized flat color blocks with razor-sharp 2-to-3 tone shading boundaries, zero soft airbrushing. Thick expressive colored outlines with a vibrant bright-yellow rim-light halo around the main subject. Background & Palette: Vibrant high-saturation fiery orange-to-crimson red dramatic gradient background torn by bold dark-maroon diagonal speed lines. Subject & Composition: Extreme close-up with dramatic foreshortening on the Coastal Sorcerer's pale cool blue-grey hand wrapped in dark-grey linen wrist bandages, thrusting the curved antique brass sea-hook staff violently across the diagonal. A massive, razor-sharp electric-cyan crescent energy wave and floating glowing rune cards erupt from the brass hook, smashing into a shattering dark silhouette with a jagged white-hot comic starburst flash at the center. Keep the focal point in the central-upper safe zone, leaving bottom corners simple. Full-bleed 4:3 horizontal illustration. NO realistic textures, NO muddy grey background, NO card frame, NO text.
```

### 范例 2：技能牌（以「法术防备 Spell Ward」或「静水流深 Still Water Grace」为例 —— 半身/手部 + 电光青蓝水波背景）
```text
Official Slay the Spire card illustration art style, 2D graphic vector-like cel-shaded digital art, bold posterized flat color blocks with razor-sharp 2-to-3 tone shading boundaries, zero soft airbrushing. Thick expressive deep-blue outlines with a crisp pure-white rim-light halo around the subject. Background & Palette: Vibrant high-saturation electric cyan to deep ocean blue dramatic graphic background with stylized flat horizontal wave bands. Subject & Composition: Low-angle close-up bust of the Coastal Sorcerer (tattered wide-brimmed deep navy wizard hat, thick grey cloth face-wrap, two glowing pupil-less cyan eyes in deep shadow, Prussian-blue cloak with pale-grey jagged trim). One bandaged blue-grey hand sweeps forward, conjuring three interlocking, razor-edged translucent aqua and pure-white geometric rune barrier plates in the immediate foreground that deflect bright orange incoming slashes into white-hot jagged sparks. Central-upper focal safe zone, full-bleed 4:3 horizontal illustration. NO realistic rendering, NO dull grey sky, NO card frame, NO text.
```

### 范例 3：能力牌（以「先声 Preemptive Voice」或「法术戒律 Spell Discipline」为例 —— 能量剪影 + 霓虹紫青风暴）
```text
Official Slay the Spire card illustration art style, 2D graphic vector-like cel-shaded digital art, bold posterized flat color blocks with razor-sharp 2-to-3 tone shading boundaries, zero soft airbrushing. Background & Palette: Intense deep cosmic indigo and neon-cyan to magenta radiating energy vortex background with bold graphic lightning ribbons. Subject & Composition: Symmetrical front-facing heroic bust of the Coastal Sorcerer (wide-brimmed navy wizard hat, grey face-wrap, Prussian-blue cloak) rendered as a high-contrast dark indigo silhouette wrapped in a thick, blazing double-layered pure-white and electric-cyan outer halo outline. Both bandaged blue-grey hands are raised at chest level as a crown of levitating, razor-sharp glowing gold and cyan enchanted cards orbit around the hat brim in a dramatic radial burst. Central-upper focal safe zone, full-bleed 4:3 horizontal illustration. NO soft airbrushing, NO realistic background, NO card frame, NO text.
```
