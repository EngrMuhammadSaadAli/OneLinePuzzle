#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("3W/sz93g6+THa6VrGuDs7Ozo7e4dwc5ho6sGaJPq/33qDFI1t8oJeW/s4u3db+zn72/s7O1UCIKuvsU930TZ2oa/ljh+99lfeT4LS38JkT0n7Z4yhLwFFLILqsqXgNNPcUHsO1Dh+1eL5aC1H14iUeYp8HaHr40UYyOw7iPU32ttPiNGIKTxR2JspmZD81joILmPV391ErsuwSyz0kamn7kGWitS7WnU2ZOF7e6fHsdALJIWkT6EztFT2SDuhiNqej1amtjbpLQkB4XKLFWcOM7lLK45lKREQNojqTfpew+QJUvmrMUH59HFh4ZZup403ZH5z1KXCB8cA5lm2QJjG/TIkzT7bo4ccRv9GddNYH9Tjd7kSRT7mrKmoh0/JxwYvO/u7O3s");
        private static int[] order = new int[] { 0,7,7,3,12,9,7,9,12,11,11,12,13,13,14 };
        private static int key = 237;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
