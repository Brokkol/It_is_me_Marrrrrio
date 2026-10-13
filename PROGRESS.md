# Mario2D — заметки о прогрессе

Учебное задание от знакомого из геймдева: мини-игра «как Марио» в Unity за 2 дня.
Unity 2022.3.62f3 LTS, шаблон Universal 2D, Mac M1. Код пишу в VS Code (Copilot лучше выключить, чтобы писать самому).

## Что сделано
- **Игрок** (`PlayerController.cs`): бег A/D, прыжок, двойной прыжок (`Extra Jumps Value`),
  Fall Multiplier и Low Jump Multiplier (резкое падение, короткий прыжок при коротком нажатии).
  Проверка земли — полоска под ногами (`CheckGrounded`, OverlapBoxAll + тег Ground), видна в Scene (Gizmos).
  Материал `NoFriction` на коллайдере, чтобы не залипать на стенах.
- **Прыжок на врага**: сверху — враг уничтожается, отскок `jumpForce / 1.5f`; сбоку — перезапуск уровня.
- **Уровень** `Assets/Scenes/Level1-1.unity` — раскладка Super Mario Bros 1-1 тайлами (ямы, трубы, лестницы).
  Tilemap `Grid/Ground`: Tilemap Collider 2D + Composite (Used By Composite), Rigidbody 2D Static, тег Ground.
- **Префабы** в `Assets/Prefabs`: `Enemy` (13 шт., у каждого свой Walk Distance), `Coin` (17 шт.),
  `QuestionBlock` (13 шт., тег Block, Box Collider 2D, без Rigidbody).
- Монетки со счётчиком (`CoinManager`), DeathZone под уровнем, камера за игроком, Finish у флага.
- Редакторские инструменты (меню **Mario** в Unity, папка `Assets/Editor`):
  Build World 1-1, Place Enemies and Coins, Make Question Blocks.

## Сделано 13.10
- ?-блоки: `QuestionBlock.cs` (used, серый цвет, +монета), удар головой через `normal.y < -0.5`; на блоках можно стоять.
- Прыжок на врага через `normal.y > 0.5`, +100 очков.
- Физика: Extra Jumps 0, Gravity 3, Jump 17, Fall 2; бег на Shift (`runSpeed`).
- Гумбы (`EnemyPatrol.cs`): идут влево, разворачиваются о стены (OnCollisionStay2D), просыпаются у камеры.
- Камера только вправо, Y фиксирован (Y=5, Size=7); левый край экрана — стена.
- Финиш: WinText «LEVEL COMPLETE!», стоп управления, рестарт через 3 с (`Invoke`). Level1-1 в Build Settings.
- HUD в `CoinManager` (висит на Main Camera): SCORE (монета 200, Гумба 100), COINS, TIME 400 (на нуле — смерть).

## Что ещё можно сделать
- Гриб / большой Марио (2 «жизни», ломает кирпичи).
- Жизни и Game Over, анимация смерти.
- Спрайты, анимации, звук, декор.
