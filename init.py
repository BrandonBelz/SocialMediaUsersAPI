import os
from cryptography.hazmat.primitives import serialization
from cryptography.hazmat.primitives.asymmetric import rsa

if not os.path.exists("/app/keys/private.pem"):
    print("Generating JWT keys...")
    private_key = rsa.generate_private_key(public_exponent=65537, key_size=2048)
    with open("/app/keys/private.pem", "wb") as f:
        f.write(
            private_key.private_bytes(
                encoding=serialization.Encoding.PEM,
                format=serialization.PrivateFormat.TraditionalOpenSSL,
                encryption_algorithm=serialization.NoEncryption(),
            )
        )
    with open("/app/keys/public.pem", "wb") as f:
        f.write(
            private_key.public_key().public_bytes(
                encoding=serialization.Encoding.PEM,
                format=serialization.PublicFormat.SubjectPublicKeyInfo,
            )
        )
    print("Keys generated.")
else:
    print("Keys already exist, skipping...")
