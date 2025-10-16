import { useEffect, useState } from "react";
import axios from "axios";

export default function Profile() {
  const [profile, setProfile] = useState(null);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) return;

    axios.get("http://localhost:5221/api/User/profile", {
      headers: { Authorization: `Bearer ${token}` },
    })
    .then(res => setProfile(res.data))
    .catch(() => alert("Failed to load profile"));
  }, []);

  if (!profile) return <div className="text-center mt-20 text-gray-600">Loading profile...</div>;

  return (
    <div className="max-w-md mx-auto mt-12 bg-white p-6 rounded-lg shadow-lg text-center">
      <h2 className="text-2xl font-bold mb-4">My Profile</h2>
      <p><strong>Name:</strong> {profile.fullName}</p>
      <p><strong>Email:</strong> {profile.email}</p>
      <p><strong>Role:</strong> {profile.role}</p>
      <p><strong>Phone:</strong> {profile.phoneNumber || "N/A"}</p>
      <p><strong>Birth Date:</strong> {profile.birthDate || "N/A"}</p>
    </div>
  );
}
