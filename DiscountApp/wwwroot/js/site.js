const connection = new signalR.HubConnectionBuilder()
    .withUrl("/discountHub")
    .build();

connection.start()
    .then(() => console.log("Connected to SignalR Hub"))
    .catch(err => console.error("Connection error:", err));

async function generateCodes() {

    const count = parseInt(document.getElementById("countInput").value);
    const length = parseInt(document.getElementById("lengthInput").value);

    if (isNaN(count) || count <= 0 || count > 2000) {
        alert("Please enter a valid count (1 - 2000).");
        return;
    }

    if (isNaN(length) || (length !== 7 && length !== 8)) {
        alert("Please enter a valid length (7 or 8).");
        return;
    }

    document.getElementById("resultOutput").textContent = "Loading...";

    try {
        const codes = await connection.invoke("GenerateCodes", count, length);
        document.getElementById("resultOutput").textContent = "Generated Codes:\n" + codes.join("\n");
    } catch (err) {
        console.error("Error generating codes:", err);
        document.getElementById("resultOutput").textContent = "Error generating codes.";
    }
}
async function useCode() {

    const code = document.getElementById("codeInput").value.trim();

    if (!code) {
        alert("Please enter a discount code.");
        return;
    }

    document.getElementById("resultOutput").textContent = "Loading...";

    try {
        const result = await connection.invoke("UseCode", code);
        document.getElementById("resultOutput").textContent = result
            ? `Discount code "${code}" used successfully.`
            : `Discount code "${code}" is invalid or has already been used.`;
    } catch (err) {
        console.error("Error using code:", err);
        document.getElementById("resultOutput").textContent = "Error using discount code.";
    }
}
