//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NUnit.Framework;

//namespace The_Game
//{
//    //[TestCase]
//    public class Tests
//    {
//        [Test]
//        public void TestGoing()
//        {
//            var gs = new GameState();
//            var player = new Player(gs);
//            player.UpdatePosition();
//            var pos = player.Pos + 10 * player.Velocity;
//            player.State = PlayerState.Walking;
//            Assert.AreEqual(pos, player.Pos);
//        }

//        [Test]
//        public void TestJumping()
//        {
//            var gs = new GameState();
//            var player = new Player(gs);
//            player.UpdatePosition();
//            var pos = player.Pos + 10 * player.Velocity;
//            player.State = PlayerState.Jumping;
//            Assert.AreEqual(pos, player.Pos);
//        }
//    }
//}
