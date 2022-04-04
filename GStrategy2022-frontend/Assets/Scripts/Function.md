GameController: 調用CommandLoader

CommandLoader: 讀取json，循環判斷事件，調用PlayerAction

PlayerAction: 角色行為動畫及數據更新

HexGrid: 地圖類，調用MapUnit生成地圖，提供地圖塊位置，改變MapUnit狀態

MapUnit: 地圖塊腳本，初始化時生成裝飾及資源

initialize: 讀取init.json，初始化角色