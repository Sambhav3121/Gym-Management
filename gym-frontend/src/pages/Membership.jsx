import React, { useEffect, useMemo, useState } from "react";
import http from "../api/http";
import MembershipCard from "../components/MembershipCard";

function money(n) {
  if (n === undefined || n === null || Number.isNaN(Number(n))) return "—";
  return new Intl.NumberFormat(undefined, { style: "currency", currency: "RUB" }).format(n);
}

export default function Membership() {
  const [plans, setPlans] = useState([]);
  const [current, setCurrent] = useState(null);
  const [loading, setLoading] = useState(true);
  const [submittingId, setSubmittingId] = useState(null);
  const [error, setError] = useState("");

  useEffect(() => {
    let alive = true;
    (async () => {
      try {
        setLoading(true);

        const [plansRes, currentRes] = await Promise.all([
          http.get("/api/MembershipPlan/all"),
          http.get("/api/UserMembership/current"),
        ]);

        if (!alive) return;
        setPlans(Array.isArray(plansRes.data) ? plansRes.data : []);
        setCurrent(currentRes.data ?? null);
      } catch (e) {
        if (e?.response?.status !== 404) {
          setError("Failed to load membership plans.");
        }
      } finally {
        if (alive) setLoading(false);
      }
    })();
    return () => (alive = false);
  }, []);

  const currentPlanId = current?.id;

  async function handlePurchase(planId) {
    try {
      setSubmittingId(planId);
      await http.post("/api/UserMembership/purchase", { membershipPlanId: planId });

      const fresh = await http.get("/api/UserMembership/current");
      setCurrent(fresh.data ?? null);
    } catch (e) {
      alert(e?.response?.data?.message || "Could not purchase. Please try again.");
    } finally {
      setSubmittingId(null);
    }
  }

  const headerBox = useMemo(() => {
    if (!current) return null;

    return (
      <div className="rounded-2xl border-2 border-orange-400 bg-orange-50 p-6">
        <div className="flex items-start justify-between">
          <div>
            <h4 className="text-sm font-semibold text-gray-600">Current Membership</h4>
            <p className="text-xs text-gray-500">Your active subscription</p>
            <div className="mt-4 grid grid-cols-1 gap-6 sm:grid-cols-3">
              <div>
                <div className="text-xs text-gray-500">Plan</div>
                <div className="text-lg font-semibold">{current.planName}</div>
              </div>
              <div>
                <div className="text-xs text-gray-500">Valid Until</div>
                <div className="text-lg font-semibold">
                  {current.expiryDate
                    ? new Date(current.expiryDate).toLocaleDateString()
                    : "—"}
                </div>
              </div>
              <div>
                <div className="text-xs text-gray-500">Price</div>
                <div className="text-lg font-semibold">
                  {current.price ? `${money(current.price)}/month` : "—"}
                </div>
              </div>
            </div>
          </div>
          <span className="rounded-full bg-green-100 px-3 py-1 text-xs font-semibold text-green-800">
            Active
          </span>
        </div>
      </div>
    );
  }, [current]);

  return (
    <div className="p-6">
      <h1 className="text-3xl font-bold">Membership Plans</h1>
      <p className="mt-1 text-gray-500">Choose the perfect plan for your fitness journey</p>

      <div className="mt-6">{headerBox}</div>

      {loading ? (
        <div className="mt-10 text-sm text-gray-500">Loading...</div>
      ) : error ? (
        <div className="mt-10 rounded-lg bg-red-50 p-4 text-red-700">{error}</div>
      ) : plans.length === 0 ? (
        <div className="mt-10 text-sm text-gray-500">No Membership Plans Yet</div>
      ) : (
        <div className="mt-8 grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          {plans.map((p) => {
            const id = p.id;
            return (
              <div key={id} className="relative">
                <MembershipCard
                  title={p.name}
                  subtitle={p.description}
                  priceDisplay={money(p.price)}
                  durationLabel={`for ${p.durationInMonths} months`}
                  features={[
                    "Unlimited gym access",
                    "All classes included",
                    "Personal workout plans",
                    "Progress tracking",
                  ]}
                  isCurrent={String(currentPlanId) === String(id)}
                  onPurchase={() => handlePurchase(id)}
                />
                {submittingId === id && (
                  <div className="absolute inset-0 grid place-items-center rounded-2xl bg-white/60 text-sm">
                    Purchasing…
                  </div>
                )}
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
}
