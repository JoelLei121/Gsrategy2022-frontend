# Gsrategy2022-frontend
Frontend of GStrategy2022, made by unity



### Function

GameController: 调用CommandLoader

CommandLoader: 读取json，循环判断事件，调用PlayerAction

PlayerAction: 角色行为动画及数据更新

HexGrid: 地图类，调用MapUnit生成地图，提供地图块位置，改变MapUnit状态

MapUnit: 地图块脚本，初始化时生成装饰及资源

initialize: 读取init.json，初始化角色



### Coroutine

使用协程将动画分散至不同帧数中，同时保持程序线性运行。

#### Usage

- 只有定义为IEnumrator的函数才能使用yield(即使用coroutine)

- 使用StartCoroutine()从非IEnumrator开始调用IEnumrator
- 使用StartCoroutine()会开始调用该函数，然后**马上回到原处**继续执行
- yield return StartCoroutine()会调用该函数，原函数等待直至该函数结束

#### Ways of yield

- 使用yield return null，程序等待直至下一帧
- 使用yield return new WaitForSeconds(1f)，程序等待1秒
- 使用yield break，退出该程序(不调用则不会自行退出！)

详见：https://riptutorial.com/unity3d/example/11767/coroutines



### 在Unity的调试方法

- 自行创建测试脚本，放在Scene中的目标GameObject上
- 在Update函数中，对简单的输入事件(如滑鼠点击、键盘输入)进行判断，调用测试函数

- 使用Debug.Log(var)在Console面板中输出var

关于Update函数的使用、输入事件的判断，请自行查阅。
