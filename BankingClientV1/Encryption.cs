using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Text;

public class Encryption
{
    private Encoding _encoding;
    private IBlockCipher _blockCipher;
    private PaddedBufferedBlockCipher _cipher;
    private IBlockCipherPadding _padding;
    private String _key;

    public Encryption(String key)
    {
        _blockCipher = new AesEngine();
        _encoding = Encoding.ASCII;
        _key = key;
    }

    public void SetPadding(IBlockCipherPadding padding)
    {
        if (padding != null)
            _padding = padding;
    }

    public string Encrypt(string plain)
    {
        byte[] result = BouncyCastleCrypto(true, _encoding.GetBytes(plain));
        return Convert.ToBase64String(result);
    }

    public string Decrypt(string cipher)
    {
        byte[] result = BouncyCastleCrypto(false, Convert.FromBase64String(cipher));
        return _encoding.GetString(result);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="forEncrypt"></param>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="CryptoException"></exception>
    private byte[] BouncyCastleCrypto(bool forEncrypt, byte[] input)
    {
        try
        {
            _cipher = _padding == null ? new PaddedBufferedBlockCipher(_blockCipher) : new PaddedBufferedBlockCipher(_blockCipher, _padding);
            byte[] keyByte = _encoding.GetBytes(_key);
            _cipher.Init(forEncrypt, new KeyParameter(keyByte));
            return _cipher.DoFinal(input);
        }
        catch (Org.BouncyCastle.Crypto.CryptoException ex)
        {
            throw new CryptoException(ex.Message);
        }
    }
}