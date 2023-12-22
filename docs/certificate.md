# SSL Certificate

After cloning the repository, add development SSL certificates to the services for running them in a Docker container. Run the following commands to generate and trust the SSL certificates:

1. **Download OpenSSL**

   Navigate to https://slproweb.com/products/Win32OpenSSL.html and download OpenSSL installer.

   `(Win64 OpenSSL v3.2.0 Light)` version may differ.

<br>

2. **Edit Environment Variables**

   Add `C:\Program Files\OpenSSL-Win64\bin` to System variables. Path may change based on your installation location.

<br>

3. **Create `cert.config`**

   In a folder create `cert.config`.

<details>
    <summary>The content of the cert.config Example</summary>

```config
[req]
default_bits = 2048
prompt = no
default_md = sha256
x509_extensions = v3_req
distinguished_name = dn

[dn]
C=TR
ST=Sakarya
L=Sakarya
O=HappiSoftware
OU=NeonNinjas
emailAddress=happisoftware@gmail.com
CN = localhost

[v3_req]
subjectAltName = @alt_names

[alt_names]
DNS.1 = localhost
DNS.2 = appynox-gateway-ocelotgateway
```

</details>

<br>

4. **Create pfx and crt files**

Run the following codes in the terminal.

```bash
openssl req -new -nodes -newkey rsa:2048 -keyout appynox.key -out appynox.csr -config cert.config
openssl x509 -signkey appynox.key -in appynox.csr -req -days 365 -out appynox.crt -extfile cert.config -extensions v3_req
openssl pkcs12 -export -out appynox.pfx -inkey appynox.key -in appynox.crt -password pass:happi2023
```

<br>

5. **Adding the files to APIs**

   In Ocelot Gateway project there will be `'ssl'` folder. Move the `.pfx` file under `'ssl'` folder.

<br>

6. **Trust The Certificate**

   Just double click the appynox.pfx and `next next next`

<br>
<br>

# Read Also

- [GitHub .pfx Secrets](github.md#add-pfx-secrets-to-github-for-workflow)
