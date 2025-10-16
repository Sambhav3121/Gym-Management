import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";

export default function Login() {
  const nav = useNavigate();
  const [form, setForm] = useState({ email: "", password: "" });
  const [msg, setMsg] = useState("");
  const [loading, setLoading] = useState(false);

  const handleChange = (e) =>
    setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      const { data } = await api.post("/api/User/login", form);
      if (data?.token) localStorage.setItem("token", data.token);
      setMsg("Login successful!");
      setTimeout(() => nav("/profile"), 800);
    } catch {
      setMsg("Invalid credentials. Try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex min-h-screen font-[Poppins]">
      {/* Left Image Side */}
      <div className="hidden md:block w-1/2 relative">
        <img
          src="https://images.unsplash.com/photo-1599058917212-d750089bc07c?q=80&w=1600"
          alt="Gym background"
          className="w-full h-full object-cover"
        />
        <div className="absolute inset-0 bg-black/70 flex items-center justify-center">
          <h2 className="text-4xl text-white font-extrabold">
            Welcome Back to <span className="text-yellow-400">SPORTIFY</span>
          </h2>
        </div>
      </div>

      {/* Right Form Side */}
      <div className="flex flex-col justify-center w-full md:w-1/2 bg-black text-white p-10">
        <h1 className="text-4xl font-bold mb-6 text-center">Sign In</h1>
        {msg && <p className="text-yellow-400 text-center mb-4">{msg}</p>}

        <form onSubmit={handleSubmit} className="space-y-6 max-w-md mx-auto w-full">
          <div>
            <label className="block mb-2 text-gray-400">Email</label>
            <input
              type="email"
              name="email"
              value={form.email}
              onChange={handleChange}
              className="w-full bg-neutral-900 border border-neutral-700 rounded-lg px-4 py-3 focus:ring-2 focus:ring-yellow-500 outline-none"
            />
          </div>

          <div>
            <label className="block mb-2 text-gray-400">Password</label>
            <input
              type="password"
              name="password"
              value={form.password}
              onChange={handleChange}
              className="w-full bg-neutral-900 border border-neutral-700 rounded-lg px-4 py-3 focus:ring-2 focus:ring-yellow-500 outline-none"
            />
          </div>

          <button
            disabled={loading}
            className="w-full bg-yellow-500 hover:bg-yellow-400 text-black py-3 rounded-lg font-semibold transition"
          >
            {loading ? "Signing In..." : "Login"}
          </button>

          <p className="text-center text-sm text-gray-400 mt-4">
            New to SPORTIFY?{" "}
            <a href="/register" className="text-yellow-400 hover:underline">
              Create an account
            </a>
          </p>
        </form>
      </div>
    </div>
  );
}
