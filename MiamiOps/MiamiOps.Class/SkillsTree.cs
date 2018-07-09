namespace MiamiOps
{
    public class SkillsTree
    {
        GameHandler _gameHandlerCtx;

        public SkillsTree(GameHandler gameHandlerCtx)
        {
            _gameHandlerCtx = gameHandlerCtx;
        }

        public void Update()
        {
            if (_gameHandlerCtx.RoundObject.Player.LevelUp())
            {
                _gameHandlerCtx.RoundObject.Player.LifePlayerMax += 10;
                _gameHandlerCtx.RoundObject.Player.LifePlayer = _gameHandlerCtx.RoundObject.Player.LifePlayerMax;
                _gameHandlerCtx.RoundObject.Player.Speed += 0.005f;
            }
        }
    }
}
