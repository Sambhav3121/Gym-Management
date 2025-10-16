import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";

export default function Register() {
  const nav = useNavigate();
  const [form, setForm] = useState({
    fullName: "",
    email: "",
    password: "",
    role: "Member",
    phoneNumber: "",
    birthDate: "",
  });
  const [loading, setLoading] = useState(false);
  const [msg, setMsg] = useState("");

  const onChange = (e) =>
    setForm((f) => ({ ...f, [e.target.name]: e.target.value }));

  const onSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setMsg("");
    try {
      const { data } = await api.post("/api/User/register", form);
      // If backend returns token on register, store it
      if (data?.token) localStorage.setItem("token", data.token);
      setMsg("Registered successfully!");
      setTimeout(() => nav("/login"), 800);
    } catch (err) {
      setMsg(
        err?.response?.data?.message ||
          "Registration failed. Please check your details."
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-black text-white font-[Poppins] flex items-center justify-center px-4">
      <div className="w-full max-w-xl bg-neutral-900/80 border border-neutral-800 rounded-2xl p-8 shadow-xl">
        <h1 className="text-3xl font-extrabold text-center mb-6">
          Create your account
        </h1>

        {msg && (
          <div className="mb-4 text-center text-sm text-yellow-400">{msg}</div>
        )}

        <form onSubmit={onSubmit} className="space-y-4">
          <div>
            <label className="block mb-1 text-sm text-neutral-300">Full Name</label>
            <input
              className="w-full bg-black border border-neutral-700 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-yellow-500"
              name="fullName"
              value={form.fullName}
              onChange={onChange}
              required
            />
          </div>

          <div>
            <label className="block mb-1 text-sm text-neutral-300">Email</label>
            <input
              type="email"
              className="w-full bg-black border border-neutral-700 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-yellow-500"
              name="email"
              value={form.email}
              onChange={onChange}
              required
            />
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block mb-1 text-sm text-neutral-300">Password</label>
              <input
                type="password"
                className="w-full bg-black border border-neutral-700 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-yellow-500"
                name="password"
                value={form.password}
                onChange={onChange}
                required
              />
            </div>

            <div>
              <label className="block mb-1 text-sm text-neutral-300">Role</label>
              <select
                className="w-full bg-black border border-neutral-700 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-yellow-500"
                name="role"
                value={form.role}
                onChange={onChange}
              >
                <option>Member</option>
                <option>Trainer</option>
                <option>Admin</option>
              </select>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block mb-1 text-sm text-neutral-300">Phone Number</label>
              <input
                className="w-full bg-black border border-neutral-700 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-yellow-500"
                name="phoneNumber"
                value={form.phoneNumber}
                onChange={onChange}
              />
            </div>

            <div>
              <label className="block mb-1 text-sm text-neutral-300">Birth Date</label>
              <input
                type="date"
                className="w-full bg-black border border-neutral-700 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-yellow-500"
                name="birthDate"
                value={form.birthDate}
                onChange={onChange}
              />
            </div>
          </div>

          <button
            disabled={loading}
            className="w-full mt-2 bg-yellow-500 hover:bg-yellow-400 text-black font-semibold rounded-lg px-6 py-3 transition"
          >
            {loading ? "Registering..." : "Register"}
          </button>

          <p className="text-center text-sm text-neutral-400">
            Already have an account?{" "}
            <a href="/login" className="text-yellow-400 hover:underline">
              Sign in
            </a>
          </p>
        </form>
      </div>
    </div>
  );
}
