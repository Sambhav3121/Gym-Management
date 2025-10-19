import { useState, useEffect } from "react";
import { getProfile, updateProfile } from "../services/api";

export default function Profile() {
  const [user, setUser] = useState(null);
  const [editing, setEditing] = useState(false);
  const [form, setForm] = useState({
    fullName: "",
    email: "",
    address: "",
    phoneNumber: "",
    birthDate: "",
    gender: "",
  });

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const res = await getProfile();
        setUser(res.data);
        setForm(res.data);
      } catch (err) {
        alert("Failed to fetch profile. Please login again.");
      }
    };
    fetchUser();
  }, []);

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSave = async (e) => {
    e.preventDefault();
    try {
      const res = await updateProfile(form);
      setUser(res.data);
      setEditing(false);
      alert("Profile updated successfully!");
    } catch (err) {
      alert("Error updating profile");
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    window.location.href = "/login";
  };

  if (!user)
    return (
      <div className="min-h-screen bg-gray-100 flex justify-center items-center text-gray-800">
        <p>Loading profile...</p>
      </div>
    );

  return (
    <div className="min-h-screen bg-gray-100 text-gray-900 flex flex-col items-center py-12 px-6">
      <div className="bg-white p-8 rounded-xl shadow-lg w-full max-w-3xl">
        <div className="flex justify-between items-center mb-6">
          <h1 className="text-3xl font-bold text-yellow-500">My Profile</h1>
          <button
            onClick={handleLogout}
            className="bg-yellow-500 text-white px-4 py-2 rounded-lg hover:bg-yellow-600 font-semibold"
          >
            Logout
          </button>
        </div>

        {!editing ? (
          <div>
            <p className="mb-2"><span className="text-gray-500">Full Name:</span> {user.fullName}</p>
            <p className="mb-2"><span className="text-gray-500">Email:</span> {user.email}</p>
            <p className="mb-2"><span className="text-gray-500">Address:</span> {user.address || "—"}</p>
            <p className="mb-2"><span className="text-gray-500">Phone:</span> {user.phoneNumber || "—"}</p>
            <p className="mb-2"><span className="text-gray-500">Birth Date:</span> {user.birthDate ? new Date(user.birthDate).toLocaleDateString() : "—"}</p>
            <p className="mb-6"><span className="text-gray-500">Role:</span> {user.role}</p>
            <button
              onClick={() => setEditing(true)}
              className="bg-yellow-500 text-white px-6 py-2 rounded-lg font-semibold hover:bg-yellow-600"
            >
              Edit Profile
            </button>
          </div>
        ) : (
          <form onSubmit={handleSave} className="space-y-4">
            <input name="fullName" value={form.fullName} onChange={handleChange}
              className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-yellow-400 outline-none" />
            <input name="email" value={form.email} onChange={handleChange}
              className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-yellow-400 outline-none" />
            <input name="address" value={form.address || ""} onChange={handleChange}
              className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-yellow-400 outline-none" />
            <input name="phoneNumber" value={form.phoneNumber || ""} onChange={handleChange}
              className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-yellow-400 outline-none" />
            <input name="birthDate" type="date" value={form.birthDate ? form.birthDate.split("T")[0] : ""} onChange={handleChange}
              className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-yellow-400 outline-none" />
            <div className="flex justify-between mt-6">
              <button type="button" onClick={() => setEditing(false)}
                className="bg-gray-300 px-6 py-2 rounded-lg hover:bg-gray-400">Cancel</button>
              <button type="submit"
                className="bg-yellow-500 text-white px-6 py-2 rounded-lg font-semibold hover:bg-yellow-600">Save Changes</button>
            </div>
          </form>
        )}
      </div>
    </div>
  );
}
