// Variabel global sederhana
VAR sudah_kenalan = false

# speaker: Lyra # portrait: Happy # chat: Normal # layout: Left 
"Halo! Aku tidak pernah melihatmu di sekitar sini sebelumnya. Siapa namamu?"

// Multiple Choice (Pilihan Ganda)
+ [Aku Aris.]
    # speaker: Aris # portrait: Happy # chat: Normal # layout: Right
    "Namaku Aris. Aku baru saja sampai dari desa seberang."
    ~ sudah_kenalan = true
    -> respon_lyra

+ [Kenapa kamu tanya-tanya?]
    # speaker: Aris # portrait: Happy # chat: Normal # layout: Right
    "Memangnya kenapa? Aku tidak harus menjawabmu, kan?"
    ~ sudah_kenalan = false
    -> respon_lyra

=== respon_lyra ===
{ sudah_kenalan: 
    #speaker: Lyra # portrait: Happy # chat: Normal # layout: Left
    "Salam kenal, Aris! Semoga kamu betah di kota ini."
- else: 
    #speaker: Lyra # potrait: Happy # chat: Normal # layout: Left
    "Galak sekali... Padahal aku cuma ingin menyapa."
}

# speaker: Lyra # portrait: Happy # chat: Normal # layout: Left
"Ngomong-ngomong, mau keliling kota bersamaku?"

* [Boleh, ayo!]
    -> keliling_kota
* [Tidak, makasih.]
    -> END

=== keliling_kota ===
# speaker: Lyra # portrait: Happy # chat: Normal # layout: Left
"Asyik! Ayo, aku tunjukkan kedai minum yang paling enak di sini.
-> END